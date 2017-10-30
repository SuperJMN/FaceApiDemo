using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using ReactiveUI;

namespace FaceDemo
{
    public class IdentifyViewModel : ReactiveObject
    {
        private readonly IFaceServiceClient client;
        private readonly Detector detector;
        private readonly ObservableAsPropertyHelper<IEnumerable<IdentificationViewModel>> imagesHelper;
        private ObservableAsPropertyHelper<bool> loadingHelper;

        public IdentifyViewModel(IFaceServiceClient client, IDialogService dialogService)
        {
            this.client = client;
            var fileSelector = new ObservableFileSelector();
            var imageSelector = new ImageSelector(fileSelector.SelectFilesCommand);
            detector = new Detector(client);
            
            SelectFilesCommand = fileSelector.SelectFilesCommand;

            var selectFilesObs = SelectFilesCommand.Publish();

            IdentifyCommand = ReactiveCommand.CreateFromTask(IdentifyFaces, imageSelector.Images.Any());

            imagesHelper = imageSelector.Images
                .Select(list => list.Select(data => new IdentificationViewModel(data.Image, data.Source)).ToList())
                .ToProperty(this, model => model.Identifications);

            IdentifyCommand.Subscribe(detections => SetIdentifications(detections));
            IdentifyCommand.ThrownExceptions.Subscribe(async exception => await dialogService.ShowException(exception));

            loadingHelper = IdentifyCommand.IsExecuting.ToProperty(this, model => model.IsLoading);

            selectFilesObs.Connect();
        }

        public bool IsLoading => loadingHelper.Value;

        private void SetIdentifications(IEnumerable<IdentifyFromFile> identifications)
        {
            foreach (var detectFromFile in identifications)
            {
                var image = Identifications.First(model => model.Source.Name == detectFromFile.Source.Name);
                image.Identifications = detectFromFile.Identifications;
            }
        }
        
        public IEnumerable<IdentificationViewModel> Identifications => imagesHelper.Value;

        private async Task<IList<IdentifyFromFile>> IdentifyFaces()
        {
            var detection = await detector.GetFaceIds(Identifications.Select(viewModel => viewModel.Source));

            var identifications = detection
                .ToObservable()
                .SelectMany(df => GetIdentifications(df))
                .ToList();

            return await identifications;
        }

        private async Task<IdentifyFromFile> GetIdentifications(DetectFromFile detect)
        {
            var identifications = detect.Faces
                .ToObservable()
                .SelectMany(face => GetIdentification(face));

            return new IdentifyFromFile
            {
                Source = detect.File,
                Identifications = await identifications.ToList(),
            };
        }

        private async Task<Identification> GetIdentification(Face face)
        {
            var groups = await client.ListPersonGroupsAsync();
            var group = groups.First();
            var identifyResults = await client.IdentifyAsync(group.PersonGroupId, new[] { face.FaceId });

            Person person = null;
            var personId = identifyResults.FirstOrDefault()?.Candidates.FirstOrDefault()?.PersonId;
            if (personId != null)
            {
                person = await client.GetPersonAsync(group.PersonGroupId, personId.Value);                
            }

            return new Identification(face, person);
        }

        public ReactiveCommand<Unit, IList<IdentifyFromFile>> IdentifyCommand { get; }

        public ReactiveCommand<Unit, IReadOnlyList<StorageFile>> SelectFilesCommand { get; }
    }
}
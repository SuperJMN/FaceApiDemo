using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.ProjectOxford.Face;
using ReactiveUI;

namespace FaceDemo
{
    public class GroupViewModel : ReactiveObject
    {
        private readonly IFaceServiceClient client;
        private readonly ObservableAsPropertyHelper<IList<ImageData>> imagesHelper;
        private readonly ObservableAsPropertyHelper<IEnumerable<Group>> groupsHelper;
        private readonly Detector faceDetector;
        private readonly ObservableAsPropertyHelper<bool> loadingHelper;

        public GroupViewModel(IFaceServiceClient client, IDialogService dialogService)
        {
            this.client = client;
            var fileSelector = new ObservableFileSelector();
            var imageSelector = new ImageSelector(fileSelector.SelectFilesCommand);
            faceDetector = new Detector(client);

            SelectFilesCommand = fileSelector.SelectFilesCommand;

            var selectFilesObs = SelectFilesCommand.Publish();
            
            GroupCommand = ReactiveCommand.CreateFromTask(GroupImages, imageSelector.Images.Any());
            GroupCommand.ThrownExceptions.Subscribe(async exception => await dialogService.ShowException(exception));

            imagesHelper = imageSelector.Images.ToProperty(this, model => model.Images);
            groupsHelper = GroupCommand.ToProperty(this, model => model.Groups);

            loadingHelper = GroupCommand.IsExecuting.ToProperty(this, model => model.IsLoading);

            selectFilesObs.Connect();
        }

        public bool IsLoading => loadingHelper.Value;

        public IEnumerable<Group> Groups => groupsHelper.Value;

        public ReactiveCommand<Unit, IEnumerable<Group>> GroupCommand { get; }

        private async Task<IEnumerable<Group>> GroupImages()
        {
            var detects = await faceDetector.GetFaceIds(Images.Select(data => data.Source));
            var guids = detects.SelectMany(d => d.Faces.Select(face => face.FaceId)).ToArray();
            var groupResult = await client.GroupAsync(guids);

            var joined = from d in detects
                join imageData in Images on d.File.Path equals imageData.Source.Path
                select new {d.File, Faces = d.Faces, imageData.Image};

            var groups = groupResult.Groups.SelectMany((g, i) =>
                g.Select(guid =>
                {
                    var match = joined.First(arg => arg.Faces.First().FaceId == guid);
                    return new
                    {
                        Group = i,
                        Image = match.Image,
                        Source = match.File,
                    };
                }));

            var grouped = groups
                .GroupBy(a => a.Group)
                .Select(a => new Group(a.Key.ToString(), a.Select(b => b.Image).ToList()));

            return grouped;
        }

        public ReactiveCommand<Unit, IReadOnlyList<StorageFile>> SelectFilesCommand { get; }

        public IList<ImageData> Images => imagesHelper.Value;        
    }
}
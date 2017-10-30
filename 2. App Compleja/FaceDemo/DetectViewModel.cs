using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Microsoft.ProjectOxford.Face;
using ReactiveUI;

namespace FaceDemo
{
    public class DetectViewModel : ReactiveObject
    {
        private readonly Detector detector;
        private readonly ObservableAsPropertyHelper<IEnumerable<DetectionViewModel>> imagesHelper;
        private readonly ObservableAsPropertyHelper<bool> loadingHelper;

        public DetectViewModel(IFaceServiceClient client, IDialogService dialogService)
        {
            var fileSelector = new ObservableFileSelector();
            var imageSelector = new ImageSelector(fileSelector.SelectFilesCommand);
            detector = new Detector(client);
            

            SelectFilesCommand = fileSelector.SelectFilesCommand;

            var selectFilesObs = SelectFilesCommand.Publish();

            DetectCommand = ReactiveCommand.CreateFromTask(DetectFaces, imageSelector.Images.Any());

            imagesHelper = imageSelector.Images
                .Select(list => list.Select(data => new DetectionViewModel(data.Image, data.Source)).ToList())
                .ToProperty(this, model => model.Images);

            DetectCommand.Subscribe(detections => SetDetections(detections));
            DetectCommand.ThrownExceptions.Subscribe(async exception => await dialogService.ShowException(exception));

            loadingHelper = DetectCommand.IsExecuting.ToProperty(this, model => model.IsLoading);

            selectFilesObs.Connect();
        }

        public bool IsLoading => loadingHelper.Value;

        private void SetDetections(IEnumerable<DetectFromFile> detections)
        {
            foreach (var detectFromFile in detections)
            {
                var image = Images.First(model => model.Source.Name == detectFromFile.File.Name);
                image.Rects = detectFromFile.Faces.Select(face => face.FaceRectangle).ToList();
            }
        }
        
        public IEnumerable<DetectionViewModel> Images => imagesHelper.Value;

        private async Task<IList<DetectFromFile>> DetectFaces()
        {
            var detectFromFiles = await detector.GetFaceIds(Images.Select(viewModel => viewModel.Source));
            return detectFromFiles;
        }

        public ReactiveCommand<Unit, IList<DetectFromFile>> DetectCommand { get; }

        public ReactiveCommand<Unit, IReadOnlyList<StorageFile>> SelectFilesCommand { get; }
    }

    public interface IDialogService
    {
        Task<IUICommand> ShowException(Exception exception);
    }
}
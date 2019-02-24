using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.ProjectOxford.Face.Contract;
using ReactiveUI;

namespace FaceDemo
{
    public class DetectionViewModel : ReactiveObject
    {
        private IList<FaceRectangle> rects;
        public BitmapImage Image { get; }
        public StorageFile Source { get; }

        public IList<FaceRectangle> Rects
        {
            get => rects;
            set => this.RaiseAndSetIfChanged(ref rects, value);
        }

        public DetectionViewModel(BitmapImage image, StorageFile source)
        {
            Image = image;
            Source = source;
        }
    }
}
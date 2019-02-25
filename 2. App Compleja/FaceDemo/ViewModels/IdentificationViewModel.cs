using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using FaceDemo.FaceApi;
using ReactiveUI;

namespace FaceDemo.ViewModels
{
    public class IdentificationViewModel : ReactiveObject
    {
        private IList<Identification> identifications;

        public IdentificationViewModel(BitmapImage image, StorageFile source)
        {
            Image = image;
            Source = source;
        }

        public BitmapImage Image { get; }
        public StorageFile Source { get; }

        public IList<Identification> Identifications
        {
            get => identifications;
            set => this.RaiseAndSetIfChanged(ref identifications, value);
        }       
    }
}
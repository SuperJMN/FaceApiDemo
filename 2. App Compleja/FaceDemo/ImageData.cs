using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace FaceDemo
{
    public class ImageData
    {
        public BitmapImage Image { get; set; }
        public StorageFile Source { get; set; }
    }
}
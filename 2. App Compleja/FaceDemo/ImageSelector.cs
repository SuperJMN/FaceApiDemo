using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace FaceDemo
{
    public class ImageSelector
    {
        public IObservable<IList<ImageData>> Images { get; }

        public ImageSelector(IObservable<IReadOnlyList<StorageFile>> fileObs)
        {
            Images = fileObs
                .SelectMany(GetBitmaps)
                .ObserveOnDispatcher();
        }

        private static async Task<IList<ImageData>> GetBitmaps(IReadOnlyList<StorageFile> files)
        {
            var enumerable = files.Select(LoadPicture);
            var whenAll = await Task.WhenAll(enumerable);
            return whenAll;
        }

        private static async Task<ImageData> LoadPicture(StorageFile file)
        {
            var bmpImage = new BitmapImage();
            var stream = await file.OpenStreamForReadAsync();
            await bmpImage.SetSourceAsync(stream.AsRandomAccessStream());
            return new ImageData { Image = bmpImage, Source = file };
        }
    }
}
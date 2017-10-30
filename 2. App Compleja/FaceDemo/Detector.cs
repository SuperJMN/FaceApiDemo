using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.ProjectOxford.Face;

namespace FaceDemo
{
    public class Detector
    {
        private readonly IFaceServiceClient client;

        public Detector(IFaceServiceClient client)
        {
            this.client = client;
        }

        public async Task<IList<DetectFromFile>> GetFaceIds(IEnumerable<StorageFile> files)
        {
            var ids = files
                .ToObservable()
                .SelectMany(GetFaceIds);

            return await ids.ToList();
        }

        private async Task<DetectFromFile> GetFaceIds(StorageFile file)
        {
            using (var p = await file.OpenStreamForReadAsync())
            {
                var faceIds = await client.DetectAsync(p);
                return new DetectFromFile(file, faceIds);
            }
        }
    }
}
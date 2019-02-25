using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using FaceDemo.Gui;
using Microsoft.ProjectOxford.Face;

namespace FaceDemo.FaceApi
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
                .SelectMany(GetFaceIds)
                .Where(x => x != null);

            return await ids.ToList();
        }

        private async Task<DetectFromFile> GetFaceIds(StorageFile file)
        {
            try
            {
                using (var imageStream = await file.OpenStreamForReadAsync())
                {
                    var faceIds = await client.DetectAsync(imageStream);
                    return new DetectFromFile(file, faceIds);
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
using Windows.Storage;
using Microsoft.ProjectOxford.Face.Contract;

namespace FaceDemo
{
    public class DetectFromFile
    {
        public StorageFile File { get; }
        public Face[] Faces { get; }

        public DetectFromFile(StorageFile file, Face[] faces)
        {
            File = file;
            Faces = faces;
        }
    }
}
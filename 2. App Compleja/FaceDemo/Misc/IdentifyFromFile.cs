using System.Collections.Generic;
using Windows.Storage;
using FaceDemo.FaceApi;
using Microsoft.ProjectOxford.Face.Contract;

namespace FaceDemo
{
    public class IdentifyFromFile
    {
        public DetectFromFile Detection { get; set; }
        public Person Person { get; set; }
        public StorageFile Source { get; set; }
        public IList<Identification> Identifications { get; set; }
    }
}
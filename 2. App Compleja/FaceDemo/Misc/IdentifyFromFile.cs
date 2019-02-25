using System.Collections.Generic;
using Windows.Storage;
using FaceDemo.FaceApi;
using FaceDemo.Gui;
using Microsoft.ProjectOxford.Face.Contract;

namespace FaceDemo.Misc
{
    public class IdentifyFromFile
    {
        public DetectFromFile Detection { get; set; }
        public Person Person { get; set; }
        public StorageFile Source { get; set; }
        public IList<Identification> Identifications { get; set; }
    }
}
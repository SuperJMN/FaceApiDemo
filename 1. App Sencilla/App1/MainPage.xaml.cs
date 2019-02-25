using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace App1
{
    public sealed partial class MainPage : Page
    {
        private readonly FaceServiceClient faceApiClient;

        public MainPage()
        {
            this.InitializeComponent();

            faceApiClient = new FaceServiceClient(Keys.ApiKey, "https://westeurope.api.cognitive.microsoft.com/face/v1.0");         
        }

        /// <summary>
        /// Set a breakpoint in this method and pay close attention to what this does :) Don't forget to set your API key!!
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///emma.jpg", UriKind.RelativeOrAbsolute));
            using (var stream = await file.OpenStreamForReadAsync())
            {
                var personGroupId = "default";

                Face[] faces = await faceApiClient.DetectAsync(stream);
                IdentifyResult[] identifyResults = await faceApiClient.IdentifyAsync(personGroupId, new[] {faces.First().FaceId});
                Guid personId = identifyResults.First().Candidates.First().PersonId;
            }
        }


        /// <summary>
        /// If we want to register a person, we can use this method as a base
        /// </summary>
        /// <returns></returns>
        public async Task CreatePersonAsync()
        {
            var personGroupId = "default";
            var newPerson = await faceApiClient.CreatePersonAsync(personGroupId, "Mario López");
            var registeredFace = await faceApiClient.AddPersonFaceAsync(personGroupId, newPerson.PersonId, "http://url.de.cara.a.regitrar");
        }
    }
}

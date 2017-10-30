using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace App1
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private FaceServiceClient faceApiClient;

        public MainPage()
        {
            this.InitializeComponent();

            faceApiClient = new FaceServiceClient(Keys.ApiKey);         
        }

        /// <summary>
        /// Pon un punto de interrupción aquí y mira lo que ocurre al arrancar la aplicación :) No te olvides de poner tu API KEY!
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///blanca.jpg", UriKind.RelativeOrAbsolute));
            using (var stream = await file.OpenStreamForReadAsync())
            {
                var personGroupId = "group1";

                Face[] faces = await faceApiClient.DetectAsync(stream);
                IdentifyResult[] identifyResults = await faceApiClient.IdentifyAsync(personGroupId, new[] {faces.First().FaceId});
                Guid personId = identifyResults.First().Candidates.First().PersonId;
            }
        }


        /// <summary>
        /// Si queremos registrar una persona, podemos usar este método como base
        /// </summary>
        /// <returns></returns>
        public async Task CreatePersonAsync()
        {
            var personGroupId = "group1";
            var newPerson = await faceApiClient.CreatePersonAsync(personGroupId, "Mario López");
            var registeredFace = await faceApiClient.AddPersonFaceAsync(personGroupId, newPerson.PersonId, "http://url.de.cara.a.regitrar");
        }
    }
}

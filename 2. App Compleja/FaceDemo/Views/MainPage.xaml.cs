using Windows.UI.Xaml.Controls;
using FaceDemo.FaceApi;
using FaceDemo.Misc;
using FaceDemo.ViewModels;
using Microsoft.ProjectOxford.Face;

namespace FaceDemo.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = new MainViewModel(CreateClient(), new DialogService());
        }

        private IFaceServiceClient CreateClient()
        {
            return new AutoRetryFaceClient(new FaceServiceClient(Constants.SubscriptionKey, "https://westeurope.api.cognitive.microsoft.com/face/v1.0"));
        }

    }
}


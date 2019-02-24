using System.Reflection;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Castle.DynamicProxy;
using FaceDemo.FaceApi;
using FaceDemo.ViewModels;
using Microsoft.ProjectOxford.Face;
using Serilog;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace FaceDemo
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
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


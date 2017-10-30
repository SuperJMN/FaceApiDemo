using System.Collections.Generic;
using Microsoft.ProjectOxford.Face;
using ReactiveUI;

namespace FaceDemo
{
    public class MainViewModel : ReactiveObject
    {
        private readonly IFaceServiceClient client;

        public MainViewModel(IFaceServiceClient client, IDialogService dialogService)
        {
            this.client = client;
            Sections = new List<SectionViewModel>
            {
                new SectionViewModel
                {
                    Name = "Detección",
                    ViewModel = new DetectViewModel(client, dialogService)
                },
                new SectionViewModel
                {
                    Name = "Identificación",
                    ViewModel = new IdentifyViewModel(client, dialogService)
                },
                new SectionViewModel
                {
                    Name = "Agrupar",
                    ViewModel = new GroupViewModel(client, dialogService)
                },
                new SectionViewModel
                {
                    Name = "Registrar",
                    ViewModel = new RegisterViewModel(client, dialogService)
                }
            };
        }

        public ICollection<SectionViewModel> Sections { get; }
    }
}
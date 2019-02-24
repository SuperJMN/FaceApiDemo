using System.Collections.Generic;
using Microsoft.ProjectOxford.Face;
using ReactiveUI;

namespace FaceDemo.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        public MainViewModel(IFaceServiceClient client, IDialogService dialogService)
        {
            Sections = new List<SectionViewModel>
            {
                new SectionViewModel
                {
                    Name = "Detect",
                    ViewModel = new DetectViewModel(client, dialogService)
                },
                new SectionViewModel
                {
                    Name = "Identify",
                    ViewModel = new IdentifyViewModel(client, dialogService)
                },
                new SectionViewModel
                {
                    Name = "Group",
                    ViewModel = new GroupViewModel(client, dialogService)
                },
                new SectionViewModel
                {
                    Name = "Registered People",
                    ViewModel = new RegisterViewModel(client, dialogService, DefaultValues.DefaultGroupName)
                }
            };
        }

        public ICollection<SectionViewModel> Sections { get; }
    }
}
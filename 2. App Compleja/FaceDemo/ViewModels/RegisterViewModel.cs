using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using ReactiveUI;

namespace FaceDemo.ViewModels
{
    public class RegisterViewModel : ReactiveObject
    {
        private readonly IFaceServiceClient client;
        private readonly string group;
        private readonly ObservableAsPropertyHelper<IList<PersonViewModel>> peopleHelper;
        private readonly ObservableAsPropertyHelper<IList<ImageData>> imagesHelper;
        private readonly ObservableAsPropertyHelper<bool> loadingHelper;
        private string name;

        public RegisterViewModel(IFaceServiceClient client, IDialogService dialogService, string group)
        {
            this.client = client;
            this.group = group;
            var fileSelector = new ObservableFileSelector();
            SelectImagesCommand = fileSelector.SelectFilesCommand;
            var imageSelector = new ImageSelector(fileSelector.SelectFilesCommand);
            imagesHelper = imageSelector.Images.ToProperty(this, r => r.Images);

            LoadPeopleCommand = ReactiveCommand.CreateFromTask(LoadPeople);
            LoadPeopleCommand.HandleErrors(dialogService);
            var canRegister = this
                .WhenAnyValue(x => x.Name, x => x.Images, (name, images) => new {name, images})
                .Select(x => !string.IsNullOrEmpty(x.name) && x.images != null && x.images.Any());

            RegisterPersonCommand = ReactiveCommand.CreateFromTask(RegisterPerson, canRegister);
            RegisterPersonCommand.HandleErrors(dialogService);

            peopleHelper = LoadPeopleCommand.Select(people => people.Select(person => new PersonViewModel(person, client, group, dialogService)).ToList()).ToProperty(this, r => r.People);
            

            loadingHelper = RegisterPersonCommand.IsExecuting.ToProperty(this, model => model.IsLoading);

            RegisterPersonCommand
                .Select(x => Unit.Default)
                .InvokeCommand(LoadPeopleCommand);
        }

        public bool IsLoading => loadingHelper.Value;

        public ReactiveCommand<Unit, Unit> RegisterPersonCommand { get; }

        private async Task<Unit> RegisterPerson()
        {
            var person = await client.CreatePersonAsync(group, Name);

            foreach (var imageData in Images)
            {
                using (var imageStream = await imageData.Source.OpenStreamForReadAsync())
                {
                    await client.AddPersonFaceAsync(group, person.PersonId, imageStream);
                }
            }

            await client.TrainPersonGroupAsync(group);
            Name = string.Empty;
            return Unit.Default;
        }

        public IList<ImageData> Images => imagesHelper.Value;

        public IList<PersonViewModel> People => peopleHelper.Value;

        public ReactiveCommand<Unit, IList<Person>> LoadPeopleCommand { get; }

        public ReactiveCommand<Unit, IReadOnlyList<StorageFile>> SelectImagesCommand { get; }

        public string Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }

        private async Task<IList<Person>> LoadPeople()
        {
            var people = await client.ListPersonsAsync(group);
            return people;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using ReactiveUI;

namespace FaceDemo
{
    public class RegisterViewModel : ReactiveObject
    {
        private readonly IFaceServiceClient client;
        private readonly ObservableAsPropertyHelper<IList<Person>> peopleHelper;
        private readonly ObservableAsPropertyHelper<IList<ImageData>> imagesHelper;
        private readonly ObservableAsPropertyHelper<bool> loadingHelper;

        public RegisterViewModel(IFaceServiceClient client, IDialogService dialogService)
        {
            this.client = client;
            var fileSelector = new ObservableFileSelector();
            SelectImagesCommand = fileSelector.SelectFilesCommand;
            var imageSelector = new ImageSelector(fileSelector.SelectFilesCommand);

            LoadPeopleCommand = ReactiveCommand.CreateFromTask(LoadPeople);
            RegisterPersonCommand = ReactiveCommand.CreateFromTask(RegisterPerson);
            RegisterPersonCommand.ThrownExceptions.Subscribe(async exception => await dialogService.ShowException(exception));

            peopleHelper = LoadPeopleCommand.ToProperty(this, r => r.People);
            imagesHelper = imageSelector.Images.ToProperty(this, r => r.Images);

            loadingHelper = RegisterPersonCommand.IsExecuting.ToProperty(this, model => model.IsLoading);
        }

        public bool IsLoading => loadingHelper.Value;

        public ReactiveCommand<Unit, Person> RegisterPersonCommand { get; }

        private async Task<Person> RegisterPerson()
        {
            var person = await client.CreatePersonAsync(Group.PersonGroupId, Name);

            foreach (var imageData in Images)
            {
                using (var imageStream = await imageData.Source.OpenStreamForReadAsync())
                {
                    await client.AddPersonFaceAsync(Group.PersonGroupId, person.PersonId, imageStream);
                }
            }

            await client.TrainPersonGroupAsync(Group.PersonGroupId);

            return new Person();
        }

        public IList<ImageData> Images => imagesHelper.Value;

        public IList<Person> People => peopleHelper.Value;

        public ReactiveCommand<Unit, IList<Person>> LoadPeopleCommand { get; }

        public ReactiveCommand<Unit, IReadOnlyList<StorageFile>> SelectImagesCommand { get; }

        public string Name { get; set; }
        public PersonGroup Group { get; private set; }

        private async Task<IList<Person>> LoadPeople()
        {
            var groups = await client.ListPersonGroupsAsync();
            Group = groups.First();
            var people = await client.ListPersonsAsync(Group.PersonGroupId);
            
            return people;
        }
    }
}
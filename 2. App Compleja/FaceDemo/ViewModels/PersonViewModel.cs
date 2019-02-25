using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using ReactiveUI;

namespace FaceDemo.ViewModels
{
    public class PersonViewModel
    {
        public Person Person { get; }

        public ReactiveCommand DeleteCommand { get; }

        public PersonViewModel(Person person, IFaceServiceClient client, string group, IDialogService dialogService)
        {
            Person = person;
            DeleteCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                await client.DeletePersonAsync(@group, person.PersonId);
                await client.TrainPersonGroupAsync(DefaultValues.DefaultGroupName);
                MessageBus.Current.SendMessage(new PersonDeletedMessage(person));
            });

            DeleteCommand.HandleErrors(dialogService);            
        }
    }
}
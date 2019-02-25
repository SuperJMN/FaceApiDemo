using Microsoft.ProjectOxford.Face.Contract;

namespace FaceDemo.ViewModels
{
    public class PersonDeletedMessage
    {
        private readonly Person person;

        public PersonDeletedMessage(Person person)
        {
            this.person = person;
        }
    }
}
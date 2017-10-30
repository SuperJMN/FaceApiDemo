using Microsoft.ProjectOxford.Face.Contract;

namespace FaceDemo
{
    public class Identification
    {
        public Face Face { get; }
        public Person Person { get; }

        public Identification(Face face, Person person)
        {
            Face = face;
            Person = person;
        }
    }
}
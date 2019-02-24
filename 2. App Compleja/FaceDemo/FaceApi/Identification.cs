using Microsoft.ProjectOxford.Face.Contract;

namespace FaceDemo.FaceApi
{
    public class Identification
    {
        public Face Face { get; }
        public Person Person { get; }

        public double Scale { get; set; } = 1D;
        

        public Identification(Face face, Person person)
        {
            Face = face;
            Person = person;
        }
    }
}
using Schedule.Interfaces;

namespace Schedule.Entity
{
    public class Subject : ISubject
    {
        private string? name;
        public string Name 
        { 
            get => name;
            set
            {
                if (value == null) throw new ArgumentNullException();
                if (value.Length == 0) throw new ArgumentException("Длина предмета: 0");
                name = value;
            }
        }

        public Subject(string name)
        {
            this.name = name;
        }
    }
}

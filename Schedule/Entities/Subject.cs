namespace Schedule.Entities
{
    public class Subject : Entity
    {
        public new const string entityName = "Subject";
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
        public string EntityName => entityName;

        public Subject(string name)
        {
            this.name = name;
        }

        private Subject() { }
    }
}

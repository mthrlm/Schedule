namespace Schedule.Entities
{
    public class Teacher : Entity
    {
        public new const string entityName = "Teacher";

        private string? name;
        private string? middleName;
        private string? surname;
        private List<Subject> subjects;

        public string Name
        {
            get => name;
            set
            {
                if (value == null) throw new ArgumentNullException();
                if (value.Length == 0) throw new ArgumentException("Длина имени: 0");
                name = value;
            }
        }
        public string? Surname
        {
            get => surname;
            set
            {
                if (value == null) throw new ArgumentNullException();
                if (value.Length == 0) throw new ArgumentException("Длина фамилии: 0");
                surname = value;
            }
        }
        public string? MiddleName
        {
            get => middleName;
            set
            {
                middleName = value;
            }
        }

        public List<Subject> Subjects { get => subjects; set { subjects = value; } }

        public string EntityName => entityName;

        public Teacher(string name, string surname, string middleName, List<Subject> subjects)
        {
            Name = name;
            MiddleName = middleName;
            Surname = surname;
            Subjects = subjects;
        }

        public Teacher(string name, string surname, List<Subject> subjects)
        {
            Name = name;
            MiddleName = null;
            Surname = surname;
            Subjects = subjects;
        }

        public Teacher(string name, string surname, string middleName)
        {
            Name = name;
            MiddleName = middleName;
            Surname = surname;
            Subjects = new List<Subject>();
        }

        public Teacher(string name, string surname)
        {
            Name = name;
            MiddleName = null;
            Surname = surname;
            Subjects = new List<Subject>();
        }

        private Teacher() { }

        public void AddSubject(Subject subject)
        {
            Subjects.Add(subject);
        }

        public bool DeleteSubject(Subject subject)
        {
            return Subjects.Remove(subject);
        }

        public void EditProperties(Teacher teacher)
        {
            this.Name = teacher.Name;
            this.Surname = teacher.Surname;
            this.MiddleName = teacher.MiddleName;
        }

        public void EditProperties(string? name, string? surname, string? middleName)
        {
            if (name != null)
                this.Name = name;
            if (surname != null)
                this.Surname = surname;
            if (middleName != null)
                this.MiddleName = middleName;
        }

        public override string ToString()
        {
            if (this.MiddleName == null)
                return this.Surname + " " + this.Name[0] + ".";
            return this.Surname + " " + this.Name[0] + ". " + this.MiddleName[0] + ".";
        }
    }
}

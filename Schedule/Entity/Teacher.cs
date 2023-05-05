using Schedule.Interfaces;

namespace Schedule.Entity
{
    internal class Teacher : ITeacher
    {
        private string? name;
        private string? middleName;
        private string? surname;
        private List<ISubject> subjects;

        public string? Name
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
                if (value == null) throw new ArgumentNullException();
                if (value.Length == 0) throw new ArgumentException("Длина отчества: 0");
                middleName = value;
            }
        }

        public List<ISubject> Subjects { get => subjects; set { subjects = value; } }

        public Teacher(string name, string surname, string middleName, List<ISubject> subjects)
        {
            Name = name;
            MiddleName = middleName;
            Surname = surname;
            Subjects = subjects;
        }

        public Teacher(string name, string surname, List<ISubject> subjects)
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
            Subjects = new List<ISubject>();
        }

        public Teacher(string name, string surname)
        {
            Name = name;
            MiddleName = null;
            Surname = surname;
            Subjects = new List<ISubject>();
        }

        public void AddSubject(ISubject subject)
        {
            Subjects.Add(subject);
        }

        public bool DeleteSubject(ISubject subject)
        {
            return Subjects.Remove(subject);
        }

        public void EditProperties(ITeacher teacher)
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
    }
}

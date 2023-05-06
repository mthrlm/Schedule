using Schedule.Entities;
using Schedule.Interfaces;
using System.Security.Principal;
using System.Xml;
using System.Xml.Serialization;

namespace Schedule
{
    public class Model : IModel
    {
        private List<Teacher> teachersList;
        private List<Subject> subjectsList;

        public List<Teacher> Teachers => teachersList;
        public List<Subject> Subjects => subjectsList;

        Type[] types = new Type[] { typeof(Teacher), typeof(Subject) };

        public static Action OnUpdate;

        public Model()
        {
            Presenter.OnDelete += DeleteData;
            Presenter.OnSave += AddData;
            this.teachersList = GetData("Teacher").Select(entity => entity as Teacher).ToList();
            this.subjectsList = GetData("Subject").Select(entity => entity as Subject).ToList();
            OnUpdate?.Invoke();
        }

        public List<Entity> GetData(string entityName)
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<Entity>), types);

            using (FileStream fs = new FileStream(entityName + "Data.xml", FileMode.OpenOrCreate))
            {
                try
                {
                    return (List<Entity>)xml.Deserialize(fs);
                }
                catch (InvalidOperationException e)
                {
                    return new List<Entity>();
                }
            }
        }

        public void WriteData()
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<Entity>), types);

            using (FileStream fs = new FileStream("TeacherData.xml", FileMode.Create))
            {
                xml.Serialize(fs, Teachers.Select(entity => entity as Entity).ToList());
            }
            using (FileStream fs = new FileStream("SubjectData.xml", FileMode.Create))
            {
                xml.Serialize(fs, Subjects.Select(entity => entity as Entity).ToList());
            }
        }

        public void AddData<T>(T entity)
        {
            if (entity.GetType() == typeof(Teacher))
                teachersList.Add(entity as Teacher);
            if (entity.GetType() == typeof(Subject))
                subjectsList.Add(entity as Subject);
            WriteData();
            OnUpdate?.Invoke();
        }

        public void DeleteData<T>(T entity)
        {
            if (entity.GetType() == typeof(Subject))
            {
                Subject s = entity as Subject;
                subjectsList.Remove(subjectsList.Find(x => x.Name == s.Name));
            }
            if (entity.GetType() == typeof(Teacher))
            {
                Teacher t = entity as Teacher;
                teachersList.Remove(teachersList.Find(x => x.Name == t.Name && x.Surname == t.Surname && x.MiddleName == t.MiddleName));
            }
            WriteData();
            OnUpdate?.Invoke();
        }

        public void UpdateData<T>(T entity)
        {
            if (entity.GetType() == typeof(Teacher))
            {
                var teacher = entity as Teacher;
                for(int i = 0; i < Teachers.Count; i++)
                {
                    if (teacher.Id == Teachers[i].Id)
                    {
                        Teachers[i] = teacher;
                    }
                }
            }

            if (entity.GetType() == typeof(Subject))
            {
                var subject = entity as Subject;
                for (int i = 0; i < Subjects.Count; i++)
                {
                    if (subject.Id == teachersList[i].Id)
                    {
                        Subjects[i] = subject;
                    }
                }
            }
            WriteData();
            OnUpdate?.Invoke();
        }
    }
}

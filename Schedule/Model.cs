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

        public Model()
        {
            this.teachersList = GetData("Teacher").Select(entity => entity as Teacher).ToList();
            this.subjectsList = GetData("Subject").Select(entity => entity as Subject).ToList();
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

            using (FileStream fs = new FileStream("TeacherData.xml", FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, Teachers.Select(entity => entity as Entity).ToList());
            }
            using (FileStream fs = new FileStream("SubjectData.xml", FileMode.OpenOrCreate))
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
        }
    }
}

using Schedule.Entities;

namespace Schedule.Interfaces
{
    public interface IModel
    {
        List<Teacher> Teachers { get; }
        List<Subject> Subjects { get; }
        List<Entity> GetData(string entityName);
        void WriteData();
    }
}

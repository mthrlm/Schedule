using Schedule.Entities;

namespace Schedule.Interfaces
{
    public interface IModel
    {
        static Action OnUpdate;
        List<Teacher> Teachers { get; }
        List<Subject> Subjects { get; }
        List<Entity> GetData(string entityName);
        void WriteData();
        void AddData<T>(T entity);
        void DeleteData<T>(T entity);
        void UpdateData<T>(T entity);
    }
}

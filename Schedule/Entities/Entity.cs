using System.Xml.Serialization;

namespace Schedule.Entities
{
    public abstract class Entity
    {
        protected int id;

        public const string entityName = "Entity";
        public static int overall_id = 0;

        public string EntityName { get { return entityName; } }
        public int Id { get; }

        protected Entity()
        {
            overall_id++;
            id = overall_id;
        }

        ~Entity()
        {
            overall_id--;
        }
    }
}

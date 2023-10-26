namespace Entities.Common
{

    public interface IEntity
    {

    }
    public class BaseEntity<TKey>:IEntity
    {
        public TKey Id { get; set; }
    }

    public class BaseEntity:IEntity
    {
        public int Id { get; set; }
    }
}

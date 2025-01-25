namespace DataProvider.EntityFramework.Entities;
public interface IBaseEntity { }

public class BaseEntity : IBaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}

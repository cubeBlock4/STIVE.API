namespace Infrastructure.Entities;

public class FamilleEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}
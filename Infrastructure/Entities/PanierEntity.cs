namespace Infrastructure.Entities;

public class PanierEntity
{
    public int Id { get; set; }
    
    public int CustomerId { get; set; } 
    public virtual CustomerEntity Customer { get; set; }
    
    public virtual ICollection<ProductEntity> Products { get; set; } =  new List<ProductEntity>();
}
namespace Infrastructure.Entities;

public class BasketEntity
{
    public int Id { get; set; }
    
    public int CustomerId { get; set; } 
    public virtual CustomerEntity Customer { get; set; }
    
    public virtual ICollection<ItemEntity> Items { get; set; } =  new List<ItemEntity>();
}
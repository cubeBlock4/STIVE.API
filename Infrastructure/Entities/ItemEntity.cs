namespace Infrastructure.Entities;

public class ItemEntity
{
    public int Id { get; set; }
    
    public int BasketId { get; set; }
    public virtual BasketEntity Basket { get; set; }
    
    public int ProductId { get; set; }
    public virtual ProductEntity Product { get; set; }
    
    public int Quantity { get; set; }
}
namespace Infrastructure.Entities;

public class ProductEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Reference { get; set; }
    public string Price { get; set; }
    
    public int FamilleId { get; set; } 
    public virtual FamilleEntity Famille { get; set; }
    
    public int SupplierId { get; set; } 
    public virtual SupplierEntity Supplier { get; set; }
}
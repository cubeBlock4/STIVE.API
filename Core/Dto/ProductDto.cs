namespace Core.Dto;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Reference { get; set; }
    public string Price { get; set; }
    
    public FamilleDto Famille { get; set; }
    public SupplierDto Supplier { get; set; }
}
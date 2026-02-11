namespace Core.Dto;

public class SupplierDto
{
    public int Id { get; set; }
    public string EntrepriseName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    
    public IEnumerable<ProductDto>? Products { get; set; } = null;
}
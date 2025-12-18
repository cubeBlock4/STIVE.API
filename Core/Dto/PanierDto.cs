namespace Core.Dto;

public class PanierDto
{
    public int Id { get; set; }
    
    public CustomerDto Customer { get; set; }
    public IEnumerable<ProductDto>? Products { get; set; } = null;
}
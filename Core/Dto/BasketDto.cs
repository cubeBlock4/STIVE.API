namespace Core.Dto;

public class BasketDto
{
    public int Id { get; set; }
    
    public CustomerDto Customer { get; set; }
    public IEnumerable<ItemDto>? Items { get; set; } = null;
}
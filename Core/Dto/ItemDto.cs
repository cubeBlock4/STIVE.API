namespace Core.Dto;

public class ItemDto
{
    public int Id { get; set; }

    public int BasketId { get; set; }
    public BasketDto Basket { get; set; }

    public int ProductId { get; set; }
    public ProductDto Product { get; set; }

    public int Quantity { get; set; }
}
namespace STIVE.Requests;

public class AddBasketItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class UpdateBasketItemRequest
{
    public int Quantity { get; set; }
}

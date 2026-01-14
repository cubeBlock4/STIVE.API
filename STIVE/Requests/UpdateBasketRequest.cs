namespace STIVE.Requests;

public class UpdateBasketRequest
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

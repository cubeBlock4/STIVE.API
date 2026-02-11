namespace STIVE.Requests.Products;

public class UpdateProductRequest
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Reference { get; set; }
    public string? Price { get; set; }
}
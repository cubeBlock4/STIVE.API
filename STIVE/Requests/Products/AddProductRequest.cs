namespace STIVE.Requests.Products;

public class AddProductRequest
{
    public string Name { get; set; }
    public string Reference { get; set; }
    public string Price { get; set; }
    public int FamilleId { get; set; }
    public int SupplierId { get; set; }
}
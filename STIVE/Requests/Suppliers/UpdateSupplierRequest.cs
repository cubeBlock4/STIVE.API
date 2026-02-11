namespace STIVE.Requests.Suppliers;

public class UpdateSupplierRequest
{
    public int Id { get; set; }
    public string? EntrepriseName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
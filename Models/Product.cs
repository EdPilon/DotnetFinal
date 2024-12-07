public class Product
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }  // Nullable string
    public int? SupplierId { get; set; }      // Nullable integer
    public int? CategoryId { get; set; }      // Nullable integer
    public string? QuantityPerUnit { get; set; }
    public decimal? UnitPrice { get; set; }
    public short? UnitsInStock { get; set; }
    public short? UnitsOnOrder { get; set; }
    public short? ReorderLevel { get; set; }
    public bool Discontinued { get; set; }
}

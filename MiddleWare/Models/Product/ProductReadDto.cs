namespace MiddleWare.Models.Product
{
    public class ProductReadDto
    {
        public string ProductName { get; set; } = null!;
        public string ProductSerialNumber { get; set; } = null!;
        public string ProductInvoice { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }
        public string Remark { get; set; } = null!;
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string CustomerCompanyName { get; set; } = null!;
    }
}

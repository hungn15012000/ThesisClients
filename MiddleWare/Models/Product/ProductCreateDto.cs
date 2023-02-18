using System.ComponentModel.DataAnnotations;

namespace MiddleWare.Models.Product
{
    public class ProductCreateDto:BaseDto
    {
        [Required]
        [StringLength(50)]
        public string ProductName { get; set; } = null!;
        [Required]
        [StringLength(50)]
        public string ProductSerialNumber { get; set; } = null!;
        [Required]
        [StringLength(50)]
        public string ProductInvoice { get; set; } = null!;
        [Required]
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }
        public string Remark { get; set; } = null!;
        [Required]
        public int CustomerId { get; set; }
    }
}

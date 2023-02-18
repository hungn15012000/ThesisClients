using System.ComponentModel.DataAnnotations;

namespace MiddleWare.Models.Customer
{
    public class CustomerUpdateDto:BaseDto
    {
        [Required]
        [StringLength(50)]
        public string? Firstname { get; set; }
        [Required]
        [StringLength(50)]
        public string? Lastname { get; set; }
        [StringLength(250)]
        public string? CompanyName { get; set; }
    }
}

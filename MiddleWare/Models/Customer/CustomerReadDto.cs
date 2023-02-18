namespace MiddleWare.Models.Customer
{
    public class CustomerReadDto : BaseDto
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? CompanyName { get; set; }
    }
}

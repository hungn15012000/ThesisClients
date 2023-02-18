using System;
using System.Collections.Generic;

namespace MiddleWare.Data
{
    public partial class Customer
    {
        public Customer()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? CompanyName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}

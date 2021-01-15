using System;

namespace Customers.Application.Models
{
    public class CustomerRequest
    {
        public string Name { get; set; }

        public int? Gender { get; set; }
        
        public int? Region { get; set; }
        
        public int? City { get; set; }

        public DateTime? PurchaseStart { get; set; }

        public DateTime? PurchaseEnd { get; set; }

        public int? Classification { get; set; }

        public int? Seller { get; set; }
    }
}
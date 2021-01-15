using System;
using System.ComponentModel.DataAnnotations;

namespace Customers.Domain.Models
{
    public class Customer
    {
        public long Id { get; set; }

        [Required, StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required, StringLength(50, MinimumLength = 2)]
        public string Phone { get; set; }
        
        public DateTime? LastPurchase { get; set; }
        
        [Required]
        public Gender Gender { get; set; }
        public long GenderId { get; set; }

        [Required]
        public City City { get; set; }
        public long CityId { get; set; }

        [Required]
        public Classification Classification { get; set; }
        public long ClassificationId { get; set; }
        
        [Required]
        public User User { get; set; }
        public long UserId { get; set; }
    }
}
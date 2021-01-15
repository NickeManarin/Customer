using System.ComponentModel.DataAnnotations;

namespace Customers.Domain.Models
{
    public class Classification
    {
        public long Id { get; set; }

        [Required, StringLength(20)]
        public string Name { get; set; }
    }
}
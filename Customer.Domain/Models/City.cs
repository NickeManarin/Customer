using System.ComponentModel.DataAnnotations;

namespace Customers.Domain.Models
{
    public class City
    {
        public long Id { get; set; }

        [Required, StringLength(20, MinimumLength = 2)]
        public string Name { get; set; }
        
        /// <summary>
        /// Multiple cities can have one region.
        /// </summary>
        public Region Region { get; set; }
        public long RegionId { get; set; }
    }
}
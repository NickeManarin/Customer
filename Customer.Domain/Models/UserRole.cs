using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Customers.Domain.Models
{
    public class UserRole
    {
        public long Id { get; set; }

        [Required, StringLength(20)]
        public string Name { get; set; }

        public bool IsAdmin { get; set; }

        /// <summary>
        /// One UserRole can be used by multiple users.
        /// </summary>
        [JsonIgnore]
        public ICollection<User> Users { get; set; }
    }
}
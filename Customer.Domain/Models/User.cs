using System;
using System.ComponentModel.DataAnnotations;

namespace Customers.Domain.Models
{
    public class User
    {
        public long Id { get; set; }

        #region Auth related properties

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required]
        public DateTime? PasswordLastUpdatedUtc { get; set; }

        #endregion

        [Required, StringLength(50, MinimumLength = 2)]
        public string Email { get; set; }
        
        /// <summary>
        /// Multiple users can have one role.
        /// </summary>
        public UserRole UserRole { get; set; }
        public long UserRoleId { get; set; }
    }
}
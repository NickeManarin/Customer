using System;

namespace Customers.Application.Models
{
    public class RefreshResponse : StatusResponse
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiryDateUtc { get; set; }
    }
}
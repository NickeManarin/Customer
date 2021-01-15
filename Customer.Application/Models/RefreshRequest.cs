namespace Customers.Application.Models
{
    public class RefreshRequest
    {
        public string Email { get; set; }
        public string RefreshToken { get; set; }
    }
}
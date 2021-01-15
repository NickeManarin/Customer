using Customers.Application.Interfaces;

namespace Customers.Application.Models
{
    public class StatusResponse : IResponse
    {
        public int Code { get; set; }
        
        public int Status { get; set; }

        public object Content { get; set; }

        public StatusResponse()
        { }
        
        public StatusResponse(int code, int status, object content)
        {
            Code = code;
            Status = status;
            Content = content;
        }
    }
}
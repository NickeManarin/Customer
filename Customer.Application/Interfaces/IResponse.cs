namespace Customers.Application.Interfaces
{
    public interface IResponse
    {
        int Code { get; set; }
        int Status { get; set; }
        object Content { get; set; }
    }
}
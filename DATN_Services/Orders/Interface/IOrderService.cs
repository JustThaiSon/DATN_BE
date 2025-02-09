using DATN_Models.DAL.Orders;
namespace DATN_Services.Orders.Interface
{
    public interface IOrderService
    {
      void AddTicket(Guid UserID, CreateTicketDAL req, out int responseCode);
    }
}

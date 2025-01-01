using DATN_Models.DAL.Orders;

namespace DATN_Models.DAO.Interface
{
    public interface IOrderDAO
    {
        void CreateOrder(Guid UserID, CreateOrderDAL req, out Guid orderDetail, out Guid orderId, out int response);
        void CreateTicket(Guid orderDetailId, TicketDAL req, out int response);
        void CreateOrderService(Guid orderId, CreateOrderServiceDAL req, out int response);
    }
}

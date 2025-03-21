using DATN_Models.DAL.Orders;

namespace DATN_Models.DAO.Interface
{
    public interface IOrderDAO
    {
        OrderMailResultDAL CreateOrder(Guid? userID, CreateOrderDAL req, out int response);
        void CreateTicket(Guid orderDetailId, TicketDAL req, out int response);
        void CreateOrderService(Guid orderId, CreateOrderServiceDAL req, out int response);
        GetDetailOrderDAL GetDetailOrder(Guid orderId, out int response);
        List<GetListTicketDAL> GetListTicket(Guid orderDetailId, out int Record, out int response);
    }
}

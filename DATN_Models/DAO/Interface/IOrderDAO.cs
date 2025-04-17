using DATN_Models.DAL.Orders;
using DATN_Models.DTOS.Order.Req;
using DATN_Models.DTOS.Order.Res;

namespace DATN_Models.DAO.Interface
{
    public interface IOrderDAO
    {
        OrderMailResultDAL CreateOrder(CreateOrderDAL req, out int response);
        void CreateTicket(Guid orderDetailId, TicketDAL req, out int response);
        void CreateOrderService(Guid orderId, CreateOrderServiceDAL req, out int response);
        GetDetailOrderDAL GetDetailOrder(Guid orderId, out int response);
        List<GetListTicketDAL> GetListTicket(Guid orderDetailId, out int Record, out int response);
        List<GetPaymentDAL> GetPayment(out int response);
        List<GetListHistoryOrderByUserRes> GetListHistoryOrderByUser(Guid userId, out int response);
        List<GetListHistoryOrderByUserRes> GetPastShowTimesByTimeFilter(Guid userId, string filterValue, out int response);
        GetOrderDetailLangdingDAL GetOrderDetailLangding(Guid orderId,out int response);
        CheckRefundDAL CheckRefund(Guid orderId, out int response);
        GetInfoRefundRes RefundOrderById(RefundOrderByIdReq req, out int response);
        List<GetInfoRefundRes> RefundByShowtime(RefundByShowtimeReq req, out int response);
    }
}

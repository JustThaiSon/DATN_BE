using DATN_Models.DAL.Orders;
using DATN_Models.DTOS.Order.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAO.Interface
{
    public interface IOrderDAO
    {
        void CreateOrder(Guid UserID, CreateOrderDAL req,out Guid orderDetail, out Guid orderId, out int response);
        void CreateTicket(Guid orderDetailId, TicketDAL req, out int response);
        void CreateOrderService(Guid orderId, CreateOrderServiceDAL req, out int response);
        GetDetailOrderDAL GetDetailOrder(Guid orderId, out int response);
        List<GetListTicketDAL> GetListTicket(Guid orderDetailId, out int Record, out int response);
    }
}

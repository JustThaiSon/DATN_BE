using DATN_Models.DAL.Orders;
using DATN_Models.DTOS.Order.Req;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Services.Orders.Interface
{
    public interface IOrderService
    {
      void AddTicket(Guid UserID, CreateTicketDAL req, out int responseCode);
    }
}

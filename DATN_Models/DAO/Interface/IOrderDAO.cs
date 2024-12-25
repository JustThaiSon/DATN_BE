using DATN_Models.DAL.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAO.Interface
{
    public interface IOrderDAO
    {
        void CreateOrder(Guid UserID,CreateOrderDAL req,out Guid orderId,out int response);
    }
}

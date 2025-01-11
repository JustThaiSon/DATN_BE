using DATN_Helpers.Common;
using DATN_Helpers.Constants;
using DATN_Helpers.Database;
using DATN_Models.DAL.Orders;
using DATN_Models.DAO.Interface;
using DATN_Models.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
namespace DATN_Models.DAO
{
    public class OrderDAO : IOrderDAO
    {
        private static string connectionString = string.Empty;

        public OrderDAO()
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }
        public void CreateOrder(Guid UserID, CreateOrderDAL req, out Guid orderDetail, out Guid orderId, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[10];
                pars[0] = new SqlParameter("@_UserId", UserID);
                pars[1] = new SqlParameter("@_TotalPrice", req.TotalPrice);
                pars[2] = new SqlParameter("@_Status", req.Status);
                pars[3] = new SqlParameter("@_IsAnonymous", req.IsAnonymous);
                pars[4] = new SqlParameter("@_PaymentId", req.PaymentId);
                pars[5] = new SqlParameter("@_QuantityTicket", req.QuantityTicket);
                pars[6] = new SqlParameter("@_TotalPriceTicket", req.TotalPriceTicket);
                pars[7] = new SqlParameter("@_OrderId", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                pars[8] = new SqlParameter("@_OrderDetailId", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                pars[9] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Order_CreateOrder", pars);
                response = ConvertUtil.ToInt(pars[9].Value);
                orderId = ConvertUtil.ToGuid(pars[7].Value);
                orderDetail = ConvertUtil.ToGuid(pars[8].Value);

            }
            catch (Exception ex)
            {
                response = -99;
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public void CreateOrderService(Guid orderId, CreateOrderServiceDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_OrderId", orderId);
                pars[1] = new SqlParameter("@_ServiceId", req.ServiceId);
                pars[2] = new SqlParameter("@_Quantity", req.Quantity);
                pars[3] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Order_OrderService", pars);
                response = ConvertUtil.ToInt(pars[3].Value);
            }
            catch (Exception ex)
            {
                response = -99;
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public void CreateTicket(Guid orderDetailId, TicketDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_OrderDetailId", orderDetailId);
                pars[1] = new SqlParameter("@_SeatByShowTimeId", req.SeatByShowTimeId);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Ticket_CreateTicket", pars);
                response = ConvertUtil.ToInt(pars[2].Value);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public GetDetailOrderDAL GetDetailOrder(Guid orderId, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_OrderId", orderId);
                pars[1] = new SqlParameter("@_OrderDetailId", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<GetDetailOrderDAL>("SP_Order_GetDetailOrder", pars);
                response = ConvertUtil.ToInt(pars[2].Value);
                var OrderDetailId = ConvertUtil.ToGuid(pars[1].Value);
                var ticket = GetListTicket(OrderDetailId, out int Record, out int res);
                if (res == (int)ResponseCodeEnum.SUCCESS)
                {
                    result.ListTicket = ticket;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public List<GetListTicketDAL> GetListTicket(Guid orderDetailId, out int Record, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_OrderDetailId", orderDetailId);
                pars[1] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetListSP<GetListTicketDAL>("SP_Ticket_GetTicket", pars);
                response = ConvertUtil.ToInt(pars[2].Value);
                Record = ConvertUtil.ToInt(pars[1].Value);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }
    }
}

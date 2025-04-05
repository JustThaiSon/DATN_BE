using DATN_Helpers.Common;
using DATN_Helpers.Constants;
using DATN_Helpers.Database;
using DATN_Models.DAL.Orders;
using DATN_Models.DAO.Interface;
using DATN_Models.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Xml.Linq;
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
        private string ConvertTicketsToXml(List<TicketDAL> tickets)
        {
            var xml = new XElement("Tickets",
                tickets.Select(t => new XElement("Ticket",
                    new XElement("SeatByShowTimeId", t.SeatByShowTimeId)
                ))
            );
            return xml.ToString();
        }
        private string ConvertServicesToXml(List<ServiceDAL> services)
        {
            var xml = new XElement("Services",
                services.Select(s => new XElement("Service",
                    new XElement("ServiceId", s.ServiceId),
                    new XElement("Quantity", s.Quantity)
                ))
            );
            return xml.ToString();
        }
        public OrderMailResultDAL CreateOrder( CreateOrderDAL req, out int response)
        {
            DBHelper db = null;
            try
            {
                string servicesXml = ConvertServicesToXml(req.Services);
                string ticketsXml = ConvertTicketsToXml(req.Tickets);

                var pars = new SqlParameter[10];
                pars[0] = new SqlParameter("@UserId", req.UserId == Guid.Empty ? DBNull.Value : req.UserId);
                pars[1] = new SqlParameter("@_Email", req.Email);
                pars[2] = new SqlParameter("@IsAnonymous", req.IsAnonymous);
                pars[3] = new SqlParameter("@PaymentId", req.PaymentId);
                pars[4] = new SqlParameter("@Services", servicesXml);
                pars[5] = new SqlParameter("@Tickets", ticketsXml);
                pars[6] = new SqlParameter("@TransactionCode", req.TransactionCode);
                pars[7] = new SqlParameter("@_VoucherCode", req.VoucherCode);
                pars[8] = new SqlParameter("@_TotalPriceMethod", req.TotalPriceMethod);
                pars[9] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<OrderMailResultDAL>("SP_Order_CreateOrder", pars);
                response = ConvertUtil.ToInt(pars[9].Value);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo đơn hàng", ex);
            }
            finally
            {
                db?.Close();
            }
        }

        public List<GetPaymentDAL> GetPayment(out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[1];
                pars[0] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetListSP<GetPaymentDAL>("SP_Order_GetPayment", pars);
                response = ConvertUtil.ToInt(pars[0].Value);
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

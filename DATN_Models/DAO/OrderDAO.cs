using Azure.Core;
using Azure;
using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Movie.Actor;
using DATN_Models.DAL.Orders;
using DATN_Models.DAO.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DATN_Models.DTOS.Order.Req;
using DATN_Models.Models;

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
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@_OrderDetailId", orderDetailId);
                pars[1] = new SqlParameter("@_ShowTimeId", req.ShowTimeId);
                pars[2] = new SqlParameter("@_SeatId", req.SeatId);
                pars[3] = new SqlParameter("@_Price", req.Price);
                pars[4] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Ticket_CreateTicket", pars);
                response = ConvertUtil.ToInt(pars[4].Value);
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

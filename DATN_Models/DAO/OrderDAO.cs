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
        public void CreateOrder(Guid UserID, CreateOrderDAL req, out Guid orderId, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[7];
                pars[0] = new SqlParameter("@_UserId", UserID);
                pars[1] = new SqlParameter("@_TotalPrice", req.TotalPrice);
                pars[2] = new SqlParameter("@_Status", req.Status);
                pars[3] = new SqlParameter("@_IsAnonymous", req.IsAnonymous);
                pars[4] = new SqlParameter("@_PaymentId", req.PaymentId);
                pars[5] = new SqlParameter("@_OrderId", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                pars[6] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Order_CreateOrder", pars);
                response = ConvertUtil.ToInt(pars[6].Value);
                orderId = ConvertUtil.ToGuid(pars[5].Value);
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
    }
}

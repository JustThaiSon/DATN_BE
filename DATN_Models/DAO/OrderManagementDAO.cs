﻿using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.OrderManagement;
using DATN_Models.DAO.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DATN_Models.DAO
{
    public class OrderManagementDAO : IOrderManagementDAO
    {
        private static string connectionString = string.Empty;

        public OrderManagementDAO()
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }

        public List<OrderManagementDAL> GetList(DateTime? fromDate, DateTime? toDate, int currentPage, int recordPerPage, out int totalRecord, out int response)
        {
            response = 0;
            totalRecord = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[6];
                pars[0] = new SqlParameter("@_FromDate", fromDate ?? (object)DBNull.Value);
                pars[1] = new SqlParameter("@_ToDate", toDate ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_CurrentPage", currentPage);
                pars[3] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[4] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[5] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetListSP<OrderManagementDAL>("SP_OrderManagement_GetList", pars);

                response = ConvertUtil.ToInt(pars[5].Value);
                totalRecord = ConvertUtil.ToInt(pars[4].Value);

                return result;
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

        public OrderManagementDetailDAL GetDetail(Guid orderId, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_OrderId", orderId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);

                // Sử dụng phương thức tùy chỉnh để lấy nhiều kết quả từ stored procedure
                SqlConnection conn = null;
                OrderManagementDetailDAL orderDetail = null;
                List<OrderTicketDetailDAL> tickets = new List<OrderTicketDetailDAL>();
                List<OrderServiceDetailDAL> services = new List<OrderServiceDetailDAL>();

                try
                {
                    conn = db.OpenConnection();
                    Console.WriteLine($"Đang truy vấn chi tiết đơn hàng với ID: {orderId}");

                    using (var command = new SqlCommand("SP_OrderManagement_GetDetail", conn))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddRange(pars);
                        command.CommandTimeout = 120; // Tăng timeout lên 2 phút

                        using (var reader = command.ExecuteReader())
                        {
                            // Đọc thông tin order
                            if (reader.HasRows && reader.Read())
                            {
                                orderDetail = new OrderManagementDetailDAL();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    try
                                    {
                                        var columnName = reader.GetName(i);
                                        var property = typeof(OrderManagementDetailDAL).GetProperty(columnName);
                                        if (property != null && !reader.IsDBNull(i))
                                        {
                                            var value = reader.GetValue(i);
                                            property.SetValue(orderDetail, value);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Lỗi khi đọc cột thứ {i}: {ex.Message}");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Không tìm thấy thông tin đơn hàng");
                            }

                            // Chuyển sang result set tiếp theo (tickets)
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    try
                                    {
                                        var ticket = new OrderTicketDetailDAL();
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            var columnName = reader.GetName(i);
                                            var property = typeof(OrderTicketDetailDAL).GetProperty(columnName);
                                            if (property != null && !reader.IsDBNull(i))
                                            {
                                                property.SetValue(ticket, reader.GetValue(i));
                                            }
                                        }
                                        tickets.Add(ticket);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Lỗi khi đọc thông tin vé: {ex.Message}");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Không có dữ liệu vé");
                            }

                            // Chuyển sang result set tiếp theo (services)
                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    try
                                    {
                                        var service = new OrderServiceDetailDAL();
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            var columnName = reader.GetName(i);
                                            var property = typeof(OrderServiceDetailDAL).GetProperty(columnName);
                                            if (property != null && !reader.IsDBNull(i))
                                            {
                                                property.SetValue(service, reader.GetValue(i));
                                            }
                                        }
                                        services.Add(service);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Lỗi khi đọc thông tin dịch vụ: {ex.Message}");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Không có dữ liệu dịch vụ");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi thực thi stored procedure: {ex.Message}");
                    throw;
                }
                finally
                {
                    if (conn != null && conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }

                if (orderDetail != null)
                {
                    orderDetail.Tickets = tickets;
                    orderDetail.Services = services;
                    Console.WriteLine($"Đã lấy thành công thông tin đơn hàng: {orderDetail.OrderCode} với {tickets.Count} vé và {services.Count} dịch vụ");
                }
                else
                {
                    Console.WriteLine("Không tìm thấy thông tin đơn hàng");
                }

                response = ConvertUtil.ToInt(pars[1].Value);
                return orderDetail;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi trong phương thức GetDetail: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
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

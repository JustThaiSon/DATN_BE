using DATN_Helpers.Common;
using DATN_Helpers.Constants;
using DATN_Helpers.Database;
using DATN_Models.DAL.Orders;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Order.Req;
using DATN_Models.DTOS.Order.Res;
using DATN_Models.Models;
using MailKit.Search;
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
        public OrderMailResultDAL CreateOrder(CreateOrderDAL req, out int response)
        {
            DBHelper db = null;
            try
            {
                string servicesXml = ConvertServicesToXml(req.Services);
                string ticketsXml = ConvertTicketsToXml(req.Tickets);

                var pars = new SqlParameter[14];
                pars[0] = new SqlParameter("@_Type", req.Type);
                pars[1] = new SqlParameter("@UserId", req.UserId == Guid.Empty ? DBNull.Value : req.UserId);
                pars[2] = new SqlParameter("@_Email", req.Email ?? (object)DBNull.Value);
                pars[3] = new SqlParameter("@IsAnonymous", req.IsAnonymous);
                pars[4] = new SqlParameter("@_UsePoint", req.PointUse);
                pars[5] = new SqlParameter("@PaymentId", req.PaymentId ?? (object)DBNull.Value);
                pars[6] = new SqlParameter("@Services", string.IsNullOrEmpty(servicesXml) ? DBNull.Value : servicesXml);
                pars[7] = new SqlParameter("@Tickets", string.IsNullOrEmpty(ticketsXml) ? DBNull.Value : ticketsXml);
                pars[8] = new SqlParameter("@TransactionCode", req.TransactionCode ?? (object)DBNull.Value);
                pars[9] = new SqlParameter("@_VoucherCode", req.VoucherCode ?? (object)DBNull.Value);
                pars[10] = new SqlParameter("@_TotalPriceMethod", req.TotalPriceMethod);
                pars[11] = new SqlParameter("@_TotalDiscount", req.TotalDiscount);
                pars[12] = new SqlParameter("@_TotalPrice", req.TotalPrice);
                pars[13] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<OrderMailResultDAL>("SP_Order_CreateOrder", pars);
                if (result != null && !string.IsNullOrWhiteSpace(result.ServiceDetailString))
                {
                    result.ServiceDetails = ServiceDetailsParser.ParseServiceDetails(result.ServiceDetailString);
                }
                response = ConvertUtil.ToInt(pars[13].Value);
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

        public List<GetListHistoryOrderByUserRes> GetListHistoryOrderByUser(Guid userId, int currentPage, int recordPerPage, out int totalRecords, out int response)
        {
            response = 0;
            totalRecords = 0; // Initialize totalRecords
            DBHelper db = null;
            List<GetListHistoryOrderByUserRes> result = new List<GetListHistoryOrderByUserRes>();

            try
            {
                var pars = new SqlParameter[5]; 
                pars[0] = new SqlParameter("@_UserId", userId);
                pars[1] = new SqlParameter("@_CurrentPage", currentPage);
                pars[2] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[3] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[4] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var dataTable = db.GetDataTableSP("SP_HistoryOrder_GetListTicket", pars);

                response = ConvertUtil.ToInt(pars[4].Value);

                totalRecords = ConvertUtil.ToInt(pars[3].Value);

                if (response == 200)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var sessionTimeParts = ConvertUtil.ToString(row["SessionTime"]).Split(" - ");
                        var sessionTime = sessionTimeParts.Length == 2 ? sessionTimeParts[0] : string.Empty;
                        var sessionDate = sessionTimeParts.Length == 2 ? sessionTimeParts[1] : string.Empty;

                        var serviceList = ConvertUtil.ToString(row["ServiceList"]).Split(',').Select(s =>
                        {
                            var parts = s.Split('|');
                            return parts.Length == 3 ? new ServiceInfoModel
                            {
                                Name = parts[0].Trim(),
                                Quantity = int.Parse(parts[1].Trim()),
                                TotalPrice = long.Parse(parts[2].Trim())
                            } : null;
                        }).Where(s => s != null).ToList();

                        var order = new GetListHistoryOrderByUserRes
                        {
                            Id = ConvertUtil.ToGuid(row["Id"]),
                            UserName = ConvertUtil.ToString(row["UserName"]),
                            MovieName = ConvertUtil.ToString(row["MovieName"]),
                            OrderCode = ConvertUtil.ToString(row["OrderCode"]),
                            CinemaName = ConvertUtil.ToString(row["CinemaName"]),
                            Address = ConvertUtil.ToString(row["Address"]),
                            Thumbnail = ConvertUtil.ToString(row["Thumbnail"]),
                            SessionTime = sessionTime,
                            SessionDate = sessionDate,
                            RoomName = ConvertUtil.ToString(row["RoomName"]),
                            SeatList = ConvertUtil.ToString(row["SeatList"]).Split(',').Select(s => s.Trim()).ToList(),
                            ServiceList = serviceList,
                            ConcessionAmount = ConvertUtil.ToLong(row["ConcessionAmount"]),
                            TotalPrice = ConvertUtil.ToLong(row["TotalPrice"]),
                            Email = ConvertUtil.ToString(row["Email"]),
                            Status = ConvertUtil.ToInt(row["Status"]),
                            CreatedDate = ConvertUtil.ToDateTime(row["CreatedDate"])
                        };
                        result.Add(order);
                    }
                }
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error while fetching order history", ex);
            }
            finally
            {
                db?.Close();
            }

            return result;
        }
        public List<GetListHistoryOrderByUserRes> GetPastShowTimesByTimeFilter(Guid userId, string filterValue, int currentPage, int recordPerPage, out int totalRecords, out int response)
        {
            response = 0;
            totalRecords = 0;
            DBHelper db = null;
            List<GetListHistoryOrderByUserRes> result = new List<GetListHistoryOrderByUserRes>();

            try
            {
                var pars = new SqlParameter[6]; // Update to 5 parameters
                pars[0] = new SqlParameter("@_UserId", userId);
                pars[1] = new SqlParameter("@_FilterValue", filterValue ?? (object)DBNull.Value);
                pars[2] = new SqlParameter("@_CurrentPage", currentPage);
                pars[3] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[4] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[5] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var dataTable = db.GetDataTableSP("SP_GetPastShowTimesByTimeFilter", pars);

                // Get response code
                response = ConvertUtil.ToInt(pars[5].Value);

                // Get total records
                totalRecords = ConvertUtil.ToInt(pars[4].Value); // Assuming the total record count is also returned in the response parameter

                if (response == 200)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var sessionTimeParts = ConvertUtil.ToString(row["SessionTime"]).Split(" - ");
                        var sessionTime = sessionTimeParts.Length == 2 ? sessionTimeParts[0] : string.Empty;
                        var sessionDate = sessionTimeParts.Length == 2 ? sessionTimeParts[1] : string.Empty;

                        var serviceList = ConvertUtil.ToString(row["ServiceList"]).Split(',').Select(s =>
                        {
                            var parts = s.Split('|');
                            return parts.Length == 3 ? new ServiceInfoModel
                            {
                                Name = parts[0].Trim(),
                                Quantity = int.Parse(parts[1].Trim()),
                                TotalPrice = long.Parse(parts[2].Trim())
                            } : null;
                        }).Where(s => s != null).ToList();

                        var showTime = new GetListHistoryOrderByUserRes
                        {
                            Id = ConvertUtil.ToGuid(row["Id"]),
                            UserName = ConvertUtil.ToString(row["UserName"]),
                            MovieName = ConvertUtil.ToString(row["MovieName"]),
                            OrderCode = ConvertUtil.ToString(row["OrderCode"]),
                            CinemaName = ConvertUtil.ToString(row["CinemaName"]),
                            Address = ConvertUtil.ToString(row["Address"]),
                            Thumbnail = ConvertUtil.ToString(row["Thumbnail"]),
                            SessionTime = sessionTime,
                            SessionDate = sessionDate,
                            RoomName = ConvertUtil.ToString(row["RoomName"]),
                            SeatList = ConvertUtil.ToString(row["SeatList"]).Split(',').Select(s => s.Trim()).ToList(),
                            ServiceList = serviceList,
                            ConcessionAmount = ConvertUtil.ToLong(row["ConcessionAmount"]),
                            TotalPrice = ConvertUtil.ToLong(row["TotalPrice"]),
                            Email = ConvertUtil.ToString(row["Email"]),
                            Status = ConvertUtil.ToInt(row["Status"]),
                            CreatedDate = ConvertUtil.ToDateTime(row["CreatedDate"])
                        };
                        result.Add(showTime);
                    }
                }
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error while fetching past show times", ex);
            }
            finally
            {
                db?.Close();
            }

            return result;
        }

        public GetOrderDetailLangdingDAL GetOrderDetailLangding(Guid orderId, out int response)
        {
            response = 0;
            DBHelper db = null;
            GetOrderDetailLangdingDAL result = null;

            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_OrderId", orderId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var dataTable = db.GetDataTableSP("SP_Order_GetOrderDetail", pars);
                response = ConvertUtil.ToInt(pars[1].Value);

                if (response == 200 && dataTable.Rows.Count > 0)
                {
                    var row = dataTable.Rows[0];

                    var sessionParts = ConvertUtil.ToString(row["SessionTime"]).Split(" - ");
                    var sessionTime = sessionParts.Length == 2 ? sessionParts[0] : string.Empty;
                    var sessionDate = sessionParts.Length == 2 ? sessionParts[1] : string.Empty;

                    var serviceList = new List<ServiceInfoRes>();
                    var rawServiceList = ConvertUtil.ToString(row["ServiceList"]);

                    if (!string.IsNullOrWhiteSpace(rawServiceList))
                    {
                        var serviceGroups = rawServiceList.Split("],");

                        foreach (var group in serviceGroups)
                        {
                            var colonIndex = group.IndexOf(':');
                            if (colonIndex == -1) continue;
                            var serviceType = group.Substring(0, colonIndex).Trim();
                            var itemsStr = group.Substring(colonIndex + 1).Trim().TrimStart('[').TrimEnd(']');
                            var items = itemsStr.Split(',');

                            foreach (var item in items)
                            {
                                var parts = item.Split(new[] { " x", " - " }, StringSplitOptions.None);
                                if (parts.Length == 3)
                                {
                                    serviceList.Add(new ServiceInfoRes
                                    {
                                        ServiceTypeName = serviceType,
                                        Name = parts[0].Trim(),
                                        Quantity = int.TryParse(parts[1].Trim(), out var qty) ? qty : 0,
                                        TotalPrice = long.TryParse(parts[2].Replace("đ", "").Trim(), out var price) ? price : 0
                                    });
                                }
                            }
                        }
                    }
                    result = new GetOrderDetailLangdingDAL
                    {
                        Id = ConvertUtil.ToGuid(row["Id"]),
                        MovieName = ConvertUtil.ToString(row["MovieName"]),
                        Duration = ConvertUtil.ToString(row["Duration"]),
                        Description = ConvertUtil.ToString(row["Description"]),
                        OrderCode = ConvertUtil.ToString(row["OrderCode"]),
                        CinemaName = ConvertUtil.ToString(row["CinemaName"]),
                        Address = ConvertUtil.ToString(row["Address"]),
                        Thumbnail = ConvertUtil.ToString(row["Thumbnail"]),
                        DiscountPrice = ConvertUtil.ToLong(row["DiscountPrice"]),
                        TotalPriceTicket = ConvertUtil.ToLong(row["TotalPriceTicket"]),
                        PointChange = ConvertUtil.ToLong(row["PointChange"]),
                        SessionTime = sessionTime,
                        SessionDate = sessionDate,
                        RoomName = ConvertUtil.ToString(row["RoomName"]),
                        SeatList = ConvertUtil.ToString(row["RawSeatList"]).Split(',').Select(s => s.Trim()).ToList(),
                        ServiceList = serviceList,
                        ConcessionAmount = ConvertUtil.ToLong(row["ConcessionAmount"]),
                        TotalPrice = ConvertUtil.ToLong(row["TotalPrice"]),
                        Email = ConvertUtil.ToString(row["Email"]),
                        CreatedDate = ConvertUtil.ToDateTime(row["CreatedDate"])
                    };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                db?.Close();
            }

            return result;
        }

        public CheckRefundDAL CheckRefund(Guid orderId, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_OrderId", orderId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<CheckRefundDAL>("SP_Order_CheckOrderUsed", pars);
                response = ConvertUtil.ToInt(pars[1].Value);
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

        public GetInfoRefundRes RefundOrderById(RefundOrderByIdReq req, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_OrderId", req.OrderId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<GetInfoRefundRes>("SP_Order_RefundOrderByOrderId", pars);
                response = ConvertUtil.ToInt(pars[1].Value);
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

        public List<GetInfoRefundRes> RefundByShowtime(RefundByShowtimeReq req, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_ShowTimeId", req.ShowtimeId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetListSP<GetInfoRefundRes>("SP_Order_RefundOrderByShowtime", pars);
                response = ConvertUtil.ToInt(pars[1].Value);
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
        public OrderMailResultDAL GetOrderSendMail(Guid orderId, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_OrderId", orderId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<OrderMailResultDAL>("SP_Order_GetOrderSendMail", pars);

                if (result != null && !string.IsNullOrWhiteSpace(result.ServiceDetailString))
                {
                    result.ServiceDetails = ServiceDetailsParser.ParseServiceDetails(result.ServiceDetailString);
                }
                response = ConvertUtil.ToInt(pars[1].Value);
                return result;
            }
            catch (Exception ex)
            {
                response = -99;
                throw new Exception("Error while fetching order details for email", ex);
            }
            finally
            {
                db?.Close();
            }
        }
    }
}

using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Cinemas;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Cinemas.Req;
using DATN_Models.DTOS.Cinemas.Res;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DATN_Models.DAO
{
    public class CinemasDAO : ICinemasDAO
    {
        private static string connectionString = string.Empty;

        public CinemasDAO()
        {
            var configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }
        public void CreateCinemas(CinemasReq resquest, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@_Name", resquest.Name);
                pars[1] = new SqlParameter("@_Address", resquest.Address);
                pars[2] = new SqlParameter("@_PhoneNumber", resquest.PhoneNumber);
                pars[3] = new SqlParameter("@_TotalRooms", resquest.TotalRooms);
                //pars[4] = new SqlParameter("@_Status", resquest.Status);
                //pars[5] = new SqlParameter("@_CreatedDate", resquest.CreatedDate);
                pars[4] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Cinema_Create", pars);
                //GetDetail GetInstanceSP
                //GetList GetListSP
                response = ConvertUtil.ToInt(pars[4].Value);
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

        public List<CinemasDAL> GetListCinemas(int currentPage, int recordPerPage, out int totalRecord, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_CurrentPage", currentPage);
                pars[1] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[2] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[3] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetListSP<CinemasDAL>("SP_Cinema_GetList", pars);
                response = ConvertUtil.ToInt(pars[3].Value);
                totalRecord = ConvertUtil.ToInt(pars[2].Value);
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

        public void UpdateCinemas(Guid CinemasId, CinemasReq request, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[7];
                pars[0] = new SqlParameter("@_CinemasId", CinemasId);
                pars[1] = new SqlParameter("@_Name", request.Name);
                pars[2] = new SqlParameter("@_Address", request.Address);
                pars[3] = new SqlParameter("@_PhoneNumber", request.PhoneNumber);
                pars[4] = new SqlParameter("@_TotalRooms", request.TotalRooms);
                pars[5] = new SqlParameter("@_Status", request.Status);
                pars[6] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Cinema_Update", pars);

                // Lấy mã phản hồi từ Stored Procedure
                response = ConvertUtil.ToInt(pars[6].Value);
            }
            catch (Exception ex)
            {
                response = -99; // Lỗi hệ thống
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public List<CinemasDAL> GetListCinemasByName(string nameCinemas, int currentPage, int recordPerPage, out int totalRecord, out int response)
        {
            response = 0;
            totalRecord = 0;
            DBHelper db = null;
            try
            {
                // Khai báo tham số truyền vào cho Stored Procedure
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@Name", nameCinemas);                           // Tên rạp chiếu phim tìm kiếm
                pars[1] = new SqlParameter("@_CurrentPage", currentPage);                   // Trang hiện tại
                pars[2] = new SqlParameter("@_RecordPerPage", recordPerPage);               // Số lượng bản ghi trên mỗi trang
                pars[3] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };  // Mã phản hồi
                pars[4] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output }; // Tổng số bản ghi tìm được

                // Kết nối đến database và gọi Stored Procedure
                db = new DBHelper(connectionString);
                var result = db.GetListSP<CinemasDAL>("SP_Cinema_SearchByName", pars);

                // Lấy mã phản hồi và tổng số bản ghi từ các tham số OUTPUT
                response = ConvertUtil.ToInt(pars[3].Value);
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
                // Đảm bảo đóng kết nối sau khi sử dụng
                if (db != null)
                    db.Close();
            }
        }

        public void UpdateCinemasAdress(Guid CinemasId, string newAddress, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                // Khai báo tham số Stored Procedure
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_CinemasId", CinemasId); // ID của rạp
                pars[1] = new SqlParameter("@_NewAddress", newAddress); // Địa chỉ mới
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output }; // Mã phản hồi OUTPUT

                // Thực thi Stored Procedure
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Cinema_UpdateAddress", pars);

                // Lấy mã phản hồi từ tham số OUTPUT
                response = ConvertUtil.ToInt(pars[2].Value);
            }
            catch (Exception ex)
            {
                // Gán mã lỗi hệ thống khi xảy ra ngoại lệ
                response = -99;
                throw new Exception("Error updating cinema address", ex);
            }
            finally
            {
                // Đảm bảo đóng kết nối
                if (db != null)
                    db.Close();
            }
        }

        public CinemasRes GetCinemaById(Guid cinemasId, out int response)
        {
            response = 0; // Khởi tạo mã phản hồi
            DBHelper db = null;

            try
            {
                // Khai báo tham số cho Stored Procedure
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_CinemasId", SqlDbType.UniqueIdentifier) { Value = cinemasId }; // Đảm bảo rằng tham số được truyền đúng loại và có giá trị
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output }; // Mã phản hồi OUTPUT

                // Thực thi Stored Procedure
                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<CinemasRes>("SP_Cinema_GetById", pars); // Đảm bảo tham số được truyền đúng

                // Lấy mã phản hồi từ tham số OUTPUT
                response = ConvertUtil.ToInt(pars[1].Value);

                // Trả về kết quả
                return result;
            }
            catch (Exception ex)
            {
                // Gán mã lỗi hệ thống khi xảy ra ngoại lệ
                response = -99;
                throw new Exception("Error fetching cinema by ID", ex);
            }
            finally
            {
                // Đảm bảo đóng kết nối
                if (db != null)
                    db.Close();
            }
        }






    }
}
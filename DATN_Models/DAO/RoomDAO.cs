﻿using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Room;
using DATN_Models.DAO.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DATN_Models.DAO
{
    public class RoomDAO : IRoomDAO
    {
        private static string connectionString = string.Empty;

        public RoomDAO()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }
        public void CreateRoom(CreateRoomDAL resquest, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[7];
                pars[0] = new SqlParameter("@_CinemaId", resquest.CinemaId);
                pars[1] = new SqlParameter("@_RoomTypeId", resquest.RoomTypeId);
                pars[2] = new SqlParameter("@_Name", resquest.Name);
                pars[3] = new SqlParameter("@_TotalColNumber", resquest.TotalColNumber);
                pars[4] = new SqlParameter("@_TotalRowNumber", resquest.TotalRowNumber);
                pars[5] = new SqlParameter("@_SeatPrice", resquest.SeatPrice);
                pars[6] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Room_Create", pars);

                response = ConvertUtil.ToInt(pars[6].Value);
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
        public void SaveSession(Guid userId)
        {

            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@key", SqlDbType.NVarChar) { Value = "UserId" };
                pars[1] = new SqlParameter("@value", SqlDbType.UniqueIdentifier) { Value = userId };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("sp_set_session_context", pars);
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

        public List<ListRoomDAL> GetListRoom(int currentPage, int recordPerPage, out int totalRecord, out int response)
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

                var result = db.GetListSP<ListRoomDAL>("SP_Room_GetList", pars);

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
        public void DeleteRoom(Guid Id, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];

                pars[0] = new SqlParameter("@_RoomId", Id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Room_Delete", pars);

                // Lấy mã phản hồi từ tham số OUTPUT
                response = ConvertUtil.ToInt(pars[1].Value);

                // Nếu mã lỗi là -102 (có lịch chiếu trong tương lai), đổi thành -202 để hiển thị thông báo phù hợp
                if (response == -102)
                {
                    response = -202;
                }
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

        public void UpdateRoom(UpdateRoomDAL req, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[6]; // Updated parameter count

                pars[0] = new SqlParameter("@_RoomId", req.Id);
                pars[1] = new SqlParameter("@_RoomTypeId", req.RoomTypeId);
                pars[2] = new SqlParameter("@_RoomName", req.Name);
                pars[3] = new SqlParameter("@_RoomStatus", req.Status);
                pars[4] = new SqlParameter("@_SeatPrice", req.SeatPrice);
                pars[5] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Room_Update", pars);

                response = ConvertUtil.ToInt(pars[5].Value);
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

        public List<ListRoomByCinemaDAL> GetListRoomByCinema(Guid CinemaID, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_CinemaID", CinemaID);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var result = db.GetListSP<ListRoomByCinemaDAL>("SP_Room_GetListByCinema", pars);

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
    }


}

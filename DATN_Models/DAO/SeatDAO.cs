﻿using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAL.Seat;
using DATN_Models.DAO.Interface.SeatAbout;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DATN_Models.DAO
{
    public class SeatDAO : ISeatDAO
    {
        private static string connectionString = string.Empty;

        public SeatDAO()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }

        public List<ListSeatDAL> GetListSeat(Guid id, int currentPage, int recordPerPage, out int totalRecord, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@_RoomId", id);
                pars[1] = new SqlParameter("@_CurrentPage", currentPage);
                pars[2] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[3] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[4] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);



                var result = db.GetListSP<ListSeatDAL>("SP_Seat_GetList", pars);


                response = ConvertUtil.ToInt(pars[4].Value);
                totalRecord = ConvertUtil.ToInt(pars[3].Value);

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

        public List<ListSeatByShowTimeDAL> GetListSeatByShowTime(Guid showTimeId, int currentPage, int recordPerPage, out int totalRecord, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@_ShowTimeId", showTimeId);
                pars[1] = new SqlParameter("@_CurrentPage", currentPage);
                pars[2] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[3] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[4] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);



                var result = db.GetListSP<ListSeatByShowTimeDAL>("SP_SeatByShowTime_GetList", pars);


                response = ConvertUtil.ToInt(pars[4].Value);
                totalRecord = ConvertUtil.ToInt(pars[3].Value);

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

        public void UpdateSeatStatus(UpdateSeatStatusDAL dataInput, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_SeatId", dataInput.Id);
                pars[1] = new SqlParameter("@_SeatStatus", dataInput.Status);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var result = db.ExecuteNonQuerySP("SP_Seat_Update_Status", pars);

                response = ConvertUtil.ToInt(pars[2].Value);
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

        public void UpdateSeatByShowTimeStatus(UpdateSeatByShowTimeStatusDAL dataInput, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_SeatByShowTimeId", dataInput.Id);
                pars[1] = new SqlParameter("@_SeatStatus", dataInput.Status);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var result = db.ExecuteNonQuerySP("SP_SeatByShowTime_Update", pars);

                response = ConvertUtil.ToInt(pars[2].Value);
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


        public void UpdateSeatType(UpdateSeatTypeDAL dataInput, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@_SeatId", dataInput.Id);
                pars[1] = new SqlParameter("@_SeatTypeId", dataInput.SeatTypeId);
                pars[2] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var result = db.ExecuteNonQuerySP("SP_Seat_Update_Type", pars);

                response = ConvertUtil.ToInt(pars[2].Value);
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

        public GetStatusByIdDAL GetStatusById(Guid Id, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_SeatId", Id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<GetStatusByIdDAL>("SP_SeatStatusById", pars);
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

        public void SetupPair(SetupPair dataInput, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_SeatId1", dataInput.Seatid1);
                pars[1] = new SqlParameter("@_SeatId2", dataInput.Seatid2);
                pars[2] = new SqlParameter("@_RoomId", dataInput.RoomId);
                pars[3] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var result = db.ExecuteNonQuerySP("SP_Seat_SetupPair", pars);

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

        public List<GetSeatByShowTimeDAL> GetSeatByShowTime(Guid showTimeId, out int totalRecord, out int response)
        {
            response = 0;
            totalRecord = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[3];
                pars[0] = new SqlParameter("@ShowTimeId", showTimeId);
                pars[1] = new SqlParameter("@TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[2] = new SqlParameter("@Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetListSP<GetSeatByShowTimeDAL>("SP_GetSeatByShowTime", pars);

                response = ConvertUtil.ToInt(pars[2].Value);
                totalRecord = ConvertUtil.ToInt(pars[1].Value);

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
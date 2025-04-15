﻿using DATN_Helpers.Extensions;
using DATN_Models.DAO.Interface;
using DATN_Models.DAL.AgeRating;
using DATN_Models.HandleData;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using DATN_Helpers.Database;
using DATN_Helpers.Common;

namespace DATN_Models.DAO
{
    public class AgeRatingDAO : IAgeRatingDAO
    {
        private readonly string connectionString;

        public AgeRatingDAO(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<AgeRatingDAL> GetAgeRatings(int currentPage, int recordPerPage, out int totalRecord, out int response)
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

                var result = db.GetListSP<AgeRatingDAL>("SP_AgeRating_GetList", pars);

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

        public AgeRatingDAL GetAgeRatingById(Guid id, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_AgeRatingId", id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                var result = db.GetInstanceSP<AgeRatingDAL>("SP_AgeRating_GetById", pars);

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

        public void CreateAgeRating(AgeRatingDAL ageRating, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[5];
                pars[0] = new SqlParameter("@_Code", ageRating.Code);
                pars[1] = new SqlParameter("@_Description", ageRating.Description);
                pars[2] = new SqlParameter("@_MinimumAge", ageRating.MinimumAge);
                pars[3] = new SqlParameter("@_Status", ageRating.Status);
                pars[4] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                db.ExecuteNonQuerySP("SP_AgeRating_Create", pars);

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

        public void UpdateAgeRating(AgeRatingDAL ageRating, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[6];
                pars[0] = new SqlParameter("@_AgeRatingId", ageRating.AgeRatingId);
                pars[1] = new SqlParameter("@_Code", ageRating.Code);
                pars[2] = new SqlParameter("@_Description", ageRating.Description);
                pars[3] = new SqlParameter("@_MinimumAge", ageRating.MinimumAge);
                pars[4] = new SqlParameter("@_Status", ageRating.Status);
                pars[5] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                db.ExecuteNonQuerySP("SP_AgeRating_Update", pars);

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

        public void DeleteAgeRating(Guid id, out int response)
        {
            response = 0;
            DBHelper db = null;

            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_AgeRatingId", id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);

                db.ExecuteNonQuerySP("SP_AgeRating_Delete", pars);

                response = ConvertUtil.ToInt(pars[1].Value);
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

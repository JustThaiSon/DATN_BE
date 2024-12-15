using Azure;
using DATN_Helpers.Common;
using DATN_Helpers.Database;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Movies.Req;
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
    public class MovieDAO : IMovieDAO
    {
        private static string connectionString = string.Empty;

        public MovieDAO()
        {
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }
        public void CreateActor(ActorReq resquest, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[6];
                pars[0] = new SqlParameter("@_Name", resquest.Name);
                pars[1] = new SqlParameter("@_DateOfBirth", resquest.DateOfBirth);
                pars[2] = new SqlParameter("@_Biography", resquest.Biography);
                pars[3] = new SqlParameter("@_Photo", resquest.Photo);
                pars[4] = new SqlParameter("@_Status", resquest.Status);
                pars[5] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Actor_Create", pars);
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
    }
}

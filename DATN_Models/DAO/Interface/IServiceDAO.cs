using DATN_Models.DAL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAO.Interface
{
    public interface IServiceDAO
    {
        void CreateService(CreateServiceDAL req ,out int response);
        void UpdateService(UpdateServiceDAL req ,out int response);
        void DeleteService(DeleteServiceDAL req ,out int response);
        List<GetServiceDAL> GetService(int currentPage, int recordPerPage, out int totalRecord, out int response);
    }
}

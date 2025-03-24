using DATN_Models.DAL;
using DATN_Models.Models;

namespace DATN_Models.DAO.Interface
{
    public interface ILogDAO
    {
        List<ChangeLog> GetLogs(int currentPage, int recordPerPage, out int totalRecord, out int response);
    }
}
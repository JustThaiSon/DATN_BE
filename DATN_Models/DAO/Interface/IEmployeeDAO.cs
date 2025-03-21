using DATN_Models.DAL.Employee;
using DATN_Models.DTOS.Employee.Req;

namespace DATN_Models.DAO.Interface
{
    public interface IEmployeeDAO
    {
        Task<int> CreateEmployee(CreateEmployeeDAL req);
        Task<int> UpdateEmployee(Guid id, UpdateEmployeeDAL req);
        Task<int> DeleteEmployee(Guid id);
        Task<(List<EmployeeDAL>, int, int)> GetListEmployee(int currentPage, int recordPerPage);
        Task<(EmployeeDAL, int)> GetEmployeeDetail(Guid id);
        Task<int> ChangePassword(ChangePasswordReq req);
    }

}


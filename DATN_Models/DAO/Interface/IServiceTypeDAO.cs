using DATN_Models.DAL.ServiceType;
using DATN_Models.DTOS.ServiceType.Req;

namespace DATN_Models.DAO.Interface
{
    public interface IServiceTypeDAO
    {
        List<ServiceTypeDAL> GetServiceTypeList(int currentPage, int recordPerPage, out int totalRecord, out int response);
        ServiceTypeDAL GetServiceTypeById(Guid id, out int response);
        void CreateServiceType(CreateServiceTypeDAL request, out int response);
        void UpdateServiceType(UpdateServiceTypeDAL request, out int response);
        void DeleteServiceType(Guid id, out int response);
    }
}
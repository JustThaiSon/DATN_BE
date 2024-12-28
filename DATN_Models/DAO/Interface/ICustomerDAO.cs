using DATN_Models.DAL.Customer;
using DATN_Models.DTOS.Customer.Req;

namespace DATN_Models.DAO.Interface
{
    public interface ICustomerDAO
    {
        #region Customer
        //void CreateMovie(AddMovieDAL req, out int response, params Guid[] actorIds);
        void UpdateCustomer(Guid Id, UpdateCustomerDAL req, out int response);
        void DeleteCustomer(Guid Id, out int response);
        void Lockout(Guid Id, out int response);
        List<GetListCustomerInformationDAL> GetListCustomer(int currentPage, int recordPerPage, out int totalRecord, out int response);
        GetListCustomerInformationDAL GetCustomerDetail(Guid Id, out int response);


        #endregion
    }
}

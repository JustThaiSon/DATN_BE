using DATN_Models.DAL.Customer;
using DATN_Models.DAL.Movie;
using DATN_Models.DTOS.Customer.Req;
using DATN_Models.DTOS.Movies.Req;
using DATN_Models.DTOS.Movies.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

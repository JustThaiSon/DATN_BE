using AutoMapper;
using DATN_BackEndApi.Extension;
using DATN_BackEndApi.Extension.CloudinarySett;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Customer;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Customer.Req;
using DATN_Models.DTOS.Customer.Res;
using DATN_Models.DTOS.Movies.Res;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[BAuthorize]
    public class CustomerController : ControllerBase
    {
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly ICustomerDAO _customerDAO;

        //private readonly ImageService _imgService;

        public CustomerController(IConfiguration configuration, IUltil ultils, IMapper mapper, ICustomerDAO customerDAO)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _customerDAO = customerDAO;
        }

        #region Nghia_Customer

        [HttpGet]
        [Route("GetCustomerList")]
        public async Task<CommonPagination<List<GetListCustomerInformationRes>>> GetCustomerList(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetListCustomerInformationRes>>();
            var result = _customerDAO.GetListCustomer(currentPage, recordPerPage, out int TotalRecord, out int response);
            var resultMapper = _mapper.Map<List<GetListCustomerInformationRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = TotalRecord;

            return res;
        }


        // Phần delete này nghĩa tạm chưa làm khi xoá khách hàng (đổi trạng thái) thì tất cả những hoá đơn các thứ cùng đổi trạng thái theo.
        [HttpPost]
        [Route("DeleteCustomer")]
        public async Task<CommonResponse<dynamic>> DeleteCustomer(Guid id)
        {
            var res = new CommonResponse<dynamic>();

            _customerDAO.DeleteCustomer(id, out int response);

            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }



        [HttpPost]
        [Route("UpdateCustomer")]
        public async Task<CommonResponse<dynamic>> UpdateCustomer(Guid id, UpdateCustomerReq req)
        {
            var res = new CommonResponse<dynamic>();
            var resultMapper = _mapper.Map<UpdateCustomerDAL>(req);

            _customerDAO.UpdateCustomer(id, resultMapper, out int response);


            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }


        [HttpPost]
        [Route("LockoutCustomer")]
        public async Task<CommonResponse<dynamic>> LockoutCustomer(Guid id)
        {
            var res = new CommonResponse<dynamic>();
            _customerDAO.Lockout(id, out int response);

            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }


        [HttpGet]
        [Route("GetCustomerDetail")]
        public async Task<CommonResponse<GetListCustomerInformationRes>> GetCustomerDetail(Guid Id)
        {
            var res = new CommonResponse<GetListCustomerInformationRes>();
            var result = _customerDAO.GetCustomerDetail(Id, out int response);
            var resultMapper = _mapper.Map<GetListCustomerInformationRes>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }


        #endregion



    }
}

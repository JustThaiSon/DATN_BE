using AutoMapper;
using DATN_BackEndApi.Extension.CloudinarySett;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.OrderManagement.Res;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderManagementController : ControllerBase
    {
        private readonly IOrderManagementDAO _orderManagementDAO;
        private readonly IMapper _mapper;
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly CloudService _cloudService;

        public OrderManagementController(IOrderManagementDAO orderManagementDAO, IUltil ultils, IMapper mapper, IConfiguration configuration)
        {
            _orderManagementDAO = orderManagementDAO;
            _mapper = mapper;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;

        }


        [HttpGet]
        [Route("GetList")]
        public CommonPagination<List<OrderManagementRes>> GetList(DateTime? fromDate, DateTime? toDate, int currentPage = 1, int recordPerPage = 10)
        {
            var res = new CommonPagination<List<OrderManagementRes>>();
            var result = _orderManagementDAO.GetList(fromDate, toDate, currentPage, recordPerPage, out int totalRecord, out int response);
            res.Data = _mapper.Map<List<OrderManagementRes>>(result);
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = totalRecord;
            return res;
        }

        [HttpGet]
        [Route("GetDetail")]
        public CommonResponse<OrderManagementDetailRes> GetDetail(Guid orderId)
        {
            var res = new CommonResponse<OrderManagementDetailRes>();
            var result = _orderManagementDAO.GetDetail(orderId, out int response);
            res.Data = _mapper.Map<OrderManagementDetailRes>(result);
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }
    }
}

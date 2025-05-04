using AutoMapper;
using DATN_BackEndApi.Extension;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Orders;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Order.Req;
using DATN_Models.DTOS.Order.Res;
using DATN_Services.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    //[BAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IOrderDAO _orderDAO;
        public OrderController(IConfiguration configuration, IUltil ultils, IMapper mapper, IOrderDAO orderDAO, IMailService mailService)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _orderDAO = orderDAO;
            _mailService = mailService;
        }
        //[HttpPost]
        //[Route("CreateOrder")]
        //public async Task<CommonResponse<string>> CreateOrder(CreateOrderReq req)
        //{
        //    var res = new CommonResponse<string>();

        //    var reqMapper = _mapper.Map<CreateOrderDAL>(req);
        //    _orderDAO.CreateOrder(GetUserId(), reqMapper, out Guid OrderId, out int response);
        //    res.Data = null;
        //    res.Message = MessageUtils.GetMessage(response, _langCode);
        //    res.ResponseCode = response;
        //    return res;
        //}


        [HttpGet]
        [Route("GetDetailOrder")]
        public async Task<CommonResponse<GetDetailOrderRes>> GetDetailOrder(Guid OrderId)
        {
            var res = new CommonResponse<GetDetailOrderRes>();
            var result = _orderDAO.GetDetailOrder(OrderId, out int response);
            var resultMapper = _mapper.Map<GetDetailOrderRes>(result);
            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }









        //[BAuthorize]
        [HttpPost]
        [Route("RefundByShowtime")]
        public async Task<CommonResponse<List<GetInfoRefundRes>>> RefundByShowtime(RefundByShowtimeReq req)
        {
            var res = new CommonResponse<List<GetInfoRefundRes>>();
            var result = _orderDAO.RefundByShowtime(req, out int response);
            if (result != null && response == 200 && result.Count() > 0)
            {
                //var userId = HttpContextHelper.GetUserId();
                foreach (var item in result)
                {
                    if (item.Email != null )
                    {
                        await _mailService.SendMailRefundAll(item);
                    }
                }
            }

            res.Data = result;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            return res;
        }



    }
}

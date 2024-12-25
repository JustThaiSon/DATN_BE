using AutoMapper;
using DATN_BackEndApi.Extension;
using DATN_BackEndApi.Extension.CloudinarySett;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Movie.Actor;
using DATN_Models.DAL.Orders;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Order.Req;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [BAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly IOrderDAO _orderDAO;
        public OrderController( IConfiguration configuration, IUltil ultils, IMapper mapper, IOrderDAO orderDAO)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _orderDAO = orderDAO;
        }
        [HttpPost]
        [Route("CreateOrder")]
        public async Task<CommonResponse<string>> CreateOrder(CreateOrderReq req)
        {
            var res = new CommonResponse<string>();

            var reqMapper = _mapper.Map<CreateOrderDAL>(req);
            _orderDAO.CreateOrder(GetUserId(),reqMapper, out Guid OrderId,out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }
        #region Define
        private Guid GetUserId()
        {
            return (Guid)(HttpContext.Items["UserId"] ?? 0);
        }
        private List<string> GetRoles()
        {
            if (HttpContext.Items["Roles"] is List<string> roles)
            {
                return roles;  // Return the list of roles
            }

            return new List<string>();
        }
        #endregion
    }
}

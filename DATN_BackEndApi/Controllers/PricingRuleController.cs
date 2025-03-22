using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.PricingRule;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.PricingRule.Req;
using DATN_Models.DTOS.PricingRule.Res;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingRuleController : ControllerBase
    {
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly IPricingRuleDAO _pricingRuleDAO;

        public PricingRuleController(IConfiguration configuration, IUltil ultils, IMapper mapper, IPricingRuleDAO pricingRuleDAO)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _pricingRuleDAO = pricingRuleDAO;
        }


        [HttpGet]
        [Route("GetAllRule")]

        public async Task<CommonPagination<List<GetListPricingRuleRes>>> GetAllRule(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetListPricingRuleRes>>();

            var result = _pricingRuleDAO.GetListPricingRule(currentPage, recordPerPage, out int TotalRecord, out int response);

            var resultMapper = _mapper.Map<List<GetListPricingRuleRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = TotalRecord;

            return res;
        }

        [HttpPost]
        [Route("CreateRule")]

        public async Task<CommonResponse<dynamic>> CreateRule( [FromBody]CreatePricingRuleReq rq)
        {
            var res = new CommonResponse<dynamic>();
            var resultMapper = _mapper.Map<CreatePricingRuleDAL>(rq);
            _pricingRuleDAO.CreatePricingRule(resultMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpPost]
        [Route("UpdateRule")]

        public async Task<CommonResponse<dynamic>> UpdateRule(UpdatePricingRuleReq rq)
        {
            var res = new CommonResponse<dynamic>();
            var resultMapper = _mapper.Map<UpdatePricingRuleDAL>(rq);
            _pricingRuleDAO.UpdatePricingRule(resultMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;

        }
        [HttpPost]
        [Route("DeleteRule")]

        public async Task<CommonResponse<dynamic>> DeleteRule(Guid id)
        {
            var res = new CommonResponse<dynamic>();
            _pricingRuleDAO.DeletePricingRule(id, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

    }
}

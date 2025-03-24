using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Logs.Res;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly ILogDAO _logDAO;

        public LogController(IConfiguration configuration, IUltil ultils, IMapper mapper, ILogDAO logDAO)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _logDAO = logDAO;
        }

        [HttpGet]
        [Route("GetLogs")]
        public async Task<CommonPagination<List<GetLogRes>>> GetLogs(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetLogRes>>();
            var result = _logDAO.GetLogs(currentPage, recordPerPage, out int totalRecord, out int response);
            var resultMapper = _mapper.Map<List<GetLogRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = totalRecord;

            return res;
        }
    }
}

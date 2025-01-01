using AutoMapper;
using DATN_BackEndApi.Extension.CloudinarySett;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Movie.Actor;
using DATN_Models.DAL.Service;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Service.Request;
using DATN_Models.DTOS.Service.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly CloudService _cloudService;
        private readonly IServiceDAO _serviceDAO;

        public ServiceController(IConfiguration configuration, IUltil ultils, IMapper mapper, CloudService imgService, IServiceDAO serviceDAO)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _cloudService = imgService;
            _serviceDAO = serviceDAO;
        }
        [HttpPost]
        [Route("CreateService")]
        public async Task<CommonResponse<string>> CreateService(IFormFile photo, [FromQuery] CreateServiceReq req)
        {
            var res = new CommonResponse<string>();
            var reqMapper = _mapper.Map<CreateServiceDAL>(req);
            if (photo != null)
            {
                reqMapper.ImageUrl = await _cloudService.UploadImageAsync(photo).ConfigureAwait(false);
            }
            _serviceDAO.CreateService(reqMapper,out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpPost]
        [Route("UpdateService")]
        public async Task<CommonResponse<string>> UpdateService(IFormFile photo, [FromQuery] UpdateServiceReq req)
        {
            var res = new CommonResponse<string>();
            var reqMapper = _mapper.Map<UpdateServiceDAL>(req);
            if (photo != null)
            {
                reqMapper.ImageUrl = await _cloudService.UploadImageAsync(photo).ConfigureAwait(false);
            }
            _serviceDAO.UpdateService(reqMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }
        [HttpPost]
        [Route("DeleteService")]
        public async Task<CommonResponse<string>> DeleteService(DeleteServiceReq req)
        {
            var res = new CommonResponse<string>();
            var reqMapper = _mapper.Map<DeleteServiceDAL>(req);
            _serviceDAO.DeleteService(reqMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }
        [HttpGet]
        [Route("GetService")]
        public async Task<CommonPagination<List<GetServiceRes>>> GetService(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetServiceRes>>();
            var result = _serviceDAO.GetService(currentPage, recordPerPage,out int totalRecord, out int response);
            var resultMapper = _mapper.Map<List<GetServiceRes>>(result);   
            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = totalRecord;
            return res;
        }
    }
}

using AutoMapper;
using DATN_BackEndApi.Extension.CloudinarySett;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Service;
using DATN_Models.DAL.ServiceType;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Service.Request;
using DATN_Models.DTOS.Service.Response;
using DATN_Models.DTOS.ServiceType.Req;
using DATN_Models.DTOS.ServiceType.Res;
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
        private readonly IServiceTypeDAO _serviceTypeDAO;

        public ServiceController(IConfiguration configuration, IUltil ultils, IMapper mapper, CloudService imgService, IServiceDAO serviceDAO, IServiceTypeDAO serviceTypeDAO)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _cloudService = imgService;
            _serviceDAO = serviceDAO;
            _serviceTypeDAO = serviceTypeDAO;
        }



        #region Service_Sơn
        [HttpPost]
        [Route("CreateService")]
        public async Task<CommonResponse<string>> CreateService(IFormFile photo, [FromQuery] CreateServiceReq req)
        {
            var res = new CommonResponse<string>();

            // Kiểm tra xem có ảnh không khi tạo mới
            if (photo == null)
            {
                res.ResponseCode = 400;
                res.Message = "Hình ảnh là bắt buộc khi tạo mới dịch vụ";
                return res;
            }

            var reqMapper = _mapper.Map<CreateServiceDAL>(req);
            reqMapper.ImageUrl = await _cloudService.UploadImageAsync(photo).ConfigureAwait(false);

            _serviceDAO.CreateService(reqMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpPost]
        [Route("UpdateService")]
        public async Task<CommonResponse<string>> UpdateService(IFormFile? photo, [FromQuery] UpdateServiceReq req)
        {
            var res = new CommonResponse<string>();
            var reqMapper = _mapper.Map<UpdateServiceDAL>(req);

            // Nếu có ảnh mới, cập nhật URL ảnh
            if (photo != null)
            {
                reqMapper.ImageUrl = await _cloudService.UploadImageAsync(photo).ConfigureAwait(false);
            }
            else
            {
                // Nếu không có ảnh mới, đặt ImageUrl thành chuỗi rỗng để thủ tục lưu trữ biết không cập nhật ảnh
                reqMapper.ImageUrl = string.Empty;
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
            var result = _serviceDAO.GetService(currentPage, recordPerPage, out int totalRecord, out int response);
            var resultMapper = _mapper.Map<List<GetServiceRes>>(result);
            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = totalRecord;
            return res;
        }

        #endregion












        #region Service_Type_Nghĩa

        [HttpGet]
        [Route("GetServiceTypeList")]
        public async Task<CommonPagination<List<ServiceTypeRes>>> GetServiceTypeList(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<ServiceTypeRes>>();
            var result = _serviceTypeDAO.GetServiceTypeList(currentPage, recordPerPage, out int totalRecord, out int response);
            var resultMapper = _mapper.Map<List<ServiceTypeRes>>(result);
            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = totalRecord;
            return res;
        }

        [HttpGet]
        [Route("GetServiceTypeById")]
        public async Task<CommonResponse<ServiceTypeRes>> GetServiceTypeById(Guid id)
        {
            var res = new CommonResponse<ServiceTypeRes>();
            var result = _serviceTypeDAO.GetServiceTypeById(id, out int response);
            var resultMapper = _mapper.Map<ServiceTypeRes>(result);
            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpPost]
        [Route("CreateServiceType")]
        public async Task<CommonResponse<string>> CreateServiceType(IFormFile photo, [FromQuery] string Name, [FromQuery] string Description)
        {
            var res = new CommonResponse<string>();

            // Kiểm tra xem có ảnh không khi tạo mới
            if (photo == null)
            {
                res.ResponseCode = 400;
                res.Message = "Hình ảnh là bắt buộc khi tạo mới loại dịch vụ";
                return res;
            }

            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Description))
            {
                res.ResponseCode = -99;
                res.Message = "The Name field is required. The Description field is required.";
                return res;
            }

            var reqMapper = new CreateServiceTypeDAL
            {
                Name = Name,
                Description = Description,
                ImageUrl = await _cloudService.UploadImageAsync(photo).ConfigureAwait(false)
            };

            _serviceTypeDAO.CreateServiceType(reqMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpPost]
        [Route("UpdateServiceType")]
        public async Task<CommonResponse<string>> UpdateServiceType(IFormFile? photo, [FromQuery] Guid Id, [FromQuery] string Name, [FromQuery] string Description)
        {
            var res = new CommonResponse<string>();

            // Kiểm tra các trường bắt buộc
            if (Id == Guid.Empty || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Description))
            {
                res.ResponseCode = -99;
                res.Message = "The Id, Name and Description fields are required.";
                return res;
            }

            var reqMapper = new UpdateServiceTypeDAL
            {
                Id = Id,
                Name = Name,
                Description = Description
            };

            // Nếu có ảnh mới, cập nhật URL ảnh
            if (photo != null)
            {
                reqMapper.ImageUrl = await _cloudService.UploadImageAsync(photo).ConfigureAwait(false);
            }
            else
            {
                // Nếu không có ảnh mới, đặt ImageUrl thành chuỗi rỗng để thủ tục lưu trữ biết không cập nhật ảnh
                reqMapper.ImageUrl = string.Empty;
            }

            _serviceTypeDAO.UpdateServiceType(reqMapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpPost]
        [Route("DeleteServiceType")]
        public async Task<CommonResponse<string>> DeleteServiceType(Guid id)
        {
            var res = new CommonResponse<string>();
            _serviceTypeDAO.DeleteServiceType(id, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        #endregion








    }
}

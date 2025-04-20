using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Employee;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Employee.Req;
using DATN_Models.DTOS.Employee.Res;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeDAO _employeeDAO;
        private readonly string _langCode;
        private readonly IMapper _mapper;
        private readonly IUltil _ultils;

        public EmployeeController(
            IEmployeeDAO employeeDAO,
            IConfiguration configuration,
            IUltil ultils,
            IMapper mapper)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _employeeDAO = employeeDAO;
        }

        [HttpPost]
        [Route("AddEmployee")]
        public async Task<CommonResponse<dynamic>> AddEmployee(CreateEmployeeReq req)
        {
            var res = new CommonResponse<dynamic>();
            var resultMapper = _mapper.Map<CreateEmployeeDAL>(req);

            int response = await _employeeDAO.CreateEmployee(resultMapper);

            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.Data = null;

            return res;
        }

        [HttpPost]
        [Route("UpdateEmployee")]
        public async Task<CommonResponse<dynamic>> UpdateEmployee(Guid id, UpdateEmployeeReq req)
        {
            var res = new CommonResponse<dynamic>();
            var resultMapper = _mapper.Map<UpdateEmployeeDAL>(req);

            int response = await _employeeDAO.UpdateEmployee(id, resultMapper);

            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.Data = null;

            return res;
        }

        [HttpPost]
        [Route("DeleteEmployee")]
        public async Task<CommonResponse<dynamic>> DeleteEmployee(Guid id)
        {
            var res = new CommonResponse<dynamic>();

            int response = await _employeeDAO.DeleteEmployee(id);

            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.Data = null;

            return res;
        }

        [HttpGet]
        [Route("GetEmployeeList")]
        public async Task<CommonPagination<List<EmployeeRes>>> GetEmployeeList(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<EmployeeRes>>();
            var (result, totalRecord, response) = await _employeeDAO.GetListEmployee(currentPage, recordPerPage);
            var resultMapper = _mapper.Map<List<EmployeeRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = totalRecord;

            return res;
        }

        [HttpGet]
        [Route("GetEmployeeDetail")]
        public async Task<CommonResponse<EmployeeRes>> GetEmployeeDetail(Guid id)
        {
            var res = new CommonResponse<EmployeeRes>();
            var (result, response) = await _employeeDAO.GetEmployeeDetail(id);
            var resultMapper = _mapper.Map<EmployeeRes>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }


        [HttpPost]
        [Route("ChangePassword")]
        public async Task<CommonResponse<dynamic>> ChangePassword(ChangePasswordReq req)
        {
            var res = new CommonResponse<dynamic>();

            int response = await _employeeDAO.ChangePassword(req);

            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.Data = null;

            return res;
        }

        [HttpPost]
        [Route("ToggleLockout")]
        public async Task<CommonResponse<dynamic>> ToggleLockoutEmployee(Guid id)
        {
            var res = new CommonResponse<dynamic>();

            int response = await _employeeDAO.ToggleLockoutEmployee(id);

            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.Data = null;

            return res;
        }
    }
}


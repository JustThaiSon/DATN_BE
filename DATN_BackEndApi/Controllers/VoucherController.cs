using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Extensions;
using DATN_Helpers.Extensions.Global;
using DATN_Models.DAL.Voucher;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Voucher.Req;
using DATN_Models.DTOS.Voucher.Res;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[BAuthorize]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherDAO _voucherDAO;
        private readonly IMapper _mapper;
        private readonly string _langCode;

        public VoucherController(IVoucherDAO voucherDAO, IMapper mapper, IConfiguration configuration)
        {
            _voucherDAO = voucherDAO;
            _mapper = mapper;
            _langCode = configuration.GetValue<string>("LanguageCode") ?? "vi";
        }

        [HttpGet]
        [Route("GetList")]
        public CommonPagination<List<VoucherRes>> GetList(
            [FromQuery] int currentPage,
            [FromQuery] int recordPerPage)
        {
            var res = new CommonPagination<List<VoucherRes>>();

            var result = _voucherDAO.GetListVoucher(currentPage, recordPerPage, out int totalRecord, out int response);

            res.Data = _mapper.Map<List<VoucherRes>>(result);
            res.TotalRecord = totalRecord;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpGet]
        [Route("GetById")]
        public CommonResponse<VoucherRes> GetById(Guid id)
        {
            var res = new CommonResponse<VoucherRes>();

            var result = _voucherDAO.GetVoucherById(id, out int response);

            res.Data = _mapper.Map<VoucherRes>(result);
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpGet]
        [Route("GetByCode")]
        public CommonResponse<VoucherRes> GetByCode(string code)
        {
            var res = new CommonResponse<VoucherRes>();

            var result = _voucherDAO.GetVoucherByCode(code, out int response);

            res.Data = _mapper.Map<VoucherRes>(result);
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpPost]
        [Route("Create")]
        public CommonResponse<string> Create([FromBody] VoucherReq request)
        {
            var res = new CommonResponse<string>();

            var voucherDAL = _mapper.Map<VoucherDAL>(request);
            _voucherDAO.CreateVoucher(voucherDAL, out int response);

            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpPost]
        [Route("Update")]
        public CommonResponse<string> Update(Guid id, [FromBody] VoucherReq request)
        {
            var res = new CommonResponse<string>();

            var voucherDAL = _mapper.Map<VoucherDAL>(request);
            voucherDAL.Id = id;

            _voucherDAO.UpdateVoucher(voucherDAL, out int response);

            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpPost]
        [Route("Delete")]
        public CommonResponse<string> Delete(Guid id)
        {
            var res = new CommonResponse<string>();

            _voucherDAO.DeleteVoucher(id, out int response);

            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpPost]
        [Route("UseVoucher")]
        public CommonResponse<string> UseVoucher([FromBody] UseVoucherReq request)
        {
            var res = new CommonResponse<string>();

            var voucherUsageDAL = _mapper.Map<VoucherUsageDAL>(request);
            _voucherDAO.UseVoucher(voucherUsageDAL, out int response);

            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpGet]
        [Route("GetUsageHistory")]
        public CommonPagination<List<VoucherUsageRes>> GetUsageHistory(
            Guid voucherId,
            [FromQuery] int currentPage,
            [FromQuery] int recordPerPage)
        {
            var res = new CommonPagination<List<VoucherUsageRes>>();

            var result = _voucherDAO.GetVoucherUsageHistory(voucherId, currentPage, recordPerPage, out int totalRecord, out int response);

            res.Data = _mapper.Map<List<VoucherUsageRes>>(result);
            res.TotalRecord = totalRecord;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }




        [HttpGet]
        [Route("GetAllUsage")]
        public CommonPagination<List<VoucherUsageRes>> GetAllUsage(
    [FromQuery] int currentPage,
    [FromQuery] int recordPerPage)
        {
            var res = new CommonPagination<List<VoucherUsageRes>>();

            var result = _voucherDAO.GetAllVoucherUsage(currentPage, recordPerPage, out int totalRecord, out int response);

            res.Data = _mapper.Map<List<VoucherUsageRes>>(result);
            res.TotalRecord = totalRecord;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }
    }
}
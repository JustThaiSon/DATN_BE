﻿using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Models.DAO.Interface;
using DATN_Models.DAL.Voucher;
using DATN_Models.DTOS.Voucher.Req;
using DATN_Models.DTOS.Voucher.Res;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DATN_Helpers.Extensions;
using DATN_LandingPage.Extension;

namespace DATN_LandingPage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserVoucherController : ControllerBase
    {
        private readonly IUserVoucherDAO _userVoucherDAO;
        private readonly IVoucherDAO _voucherDAO;
        private readonly IMapper _mapper;
        private readonly string _langCode;
        private readonly IUltil _ultil;

        public UserVoucherController(
            IUserVoucherDAO userVoucherDAO,
            IVoucherDAO voucherDAO,
            IMapper mapper,
            IConfiguration configuration,
            IUltil ultil)
        {
            _userVoucherDAO = userVoucherDAO;
            _voucherDAO = voucherDAO;
            _mapper = mapper;
            _langCode = configuration.GetValue<string>("LanguageCode") ?? "vi";
            _ultil = ultil;
        }

        [HttpGet]
        [Route("GetAvailableVouchers")]
        public CommonPagination<List<AvailableVoucherRes>> GetAvailableVouchers(
            [FromQuery] int currentPage = 1,
            [FromQuery] int recordPerPage = 10)
        {
            var res = new CommonPagination<List<AvailableVoucherRes>>();

            var result = _userVoucherDAO.GetAvailableVouchers(currentPage, recordPerPage, out int totalRecord, out int response);

            res.Data = _mapper.Map<List<AvailableVoucherRes>>(result);
            res.TotalRecord = totalRecord;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }


        [BAuthorize]
        [HttpGet]
        [Route("GetUserVouchers")]
        public CommonPagination<List<UserVoucherRes>> GetUserVouchers(
            [FromQuery] int currentPage = 1,
            [FromQuery] int recordPerPage = 10)
        {
            var res = new CommonPagination<List<UserVoucherRes>>();

            var result = _userVoucherDAO.GetUserVouchers(HttpContextHelper.GetUserId(), currentPage, recordPerPage, out int totalRecord, out int response);

            res.Data = _mapper.Map<List<UserVoucherRes>>(result);
            res.TotalRecord = totalRecord;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }



        [HttpGet]
        [Route("GetUserVoucherById")]
        public CommonResponse<UserVoucherRes> GetUserVoucherById(Guid id)
        {
            var res = new CommonResponse<UserVoucherRes>();

            var result = _userVoucherDAO.GetUserVoucherById(id, out int response);

            res.Data = _mapper.Map<UserVoucherRes>(result);
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }


        [HttpPost]
        [Route("ClaimVoucher")]
        public CommonResponse<string> ClaimVoucher([FromBody] ClaimVoucherReq request)
        {
            var res = new CommonResponse<string>();

            // Sử dụng mapper để chuyển đổi từ ClaimVoucherReq sang UserVoucherDAL
            var userVoucherDAL = _mapper.Map<UserVoucherDAL>(request);
            userVoucherDAL.UsedQuantity = 0; // Đặt giá trị mặc định cho UsedQuantity

            // Gọi trực tiếp đến stored procedure, mọi logic kiểm tra sẽ được xử lý trong SP
            _userVoucherDAO.ClaimVoucher(userVoucherDAL, out int response);

            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }



        [HttpGet]
        [Route("GetCurrentUser_DANGBILOI")]
        public CommonResponse<Guid> GetCurrentUser()
        {
            var res = new CommonResponse<Guid>();

            try
            {
                // Lấy ID của người dùng hiện tại từ token
                //var userId = _ultil.GetCurrentUserId();
                var userId = Guid.Parse("5771DD0A-67CF-4081-8926-08DD7D7E092F");

                if (userId == Guid.Empty)
                {
                    res.ResponseCode = -1;
                    res.Message = "Không tìm thấy thông tin người dùng";
                    return res;
                }

                res.Data = userId;
                res.ResponseCode = 1;
                res.Message = "Thành công";
            }
            catch (Exception ex)
            {
                res.ResponseCode = -99;
                res.Message = ex.Message;
            }

            return res;
        }
        [BAuthorize]
        [HttpPost]
        [Route("CheckVoucherAvailability")]
        public CommonResponse<string> CheckVoucherAvailability([FromBody] CheckVoucherAvailabilityReq request)
        {
            var res = new CommonResponse<string>();
            _userVoucherDAO.CheckVoucherAvailability(HttpContextHelper.GetUserId(), request.VoucherCode, out int response);
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }
    }
}

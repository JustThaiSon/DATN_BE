﻿using AutoMapper;
using DATN_BackEndApi.Extension.CloudinarySett;
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
    public class VoucherUIController : ControllerBase
    {
        private readonly IVoucherUIDAO _voucherUIDAO;
        private readonly IMapper _mapper;
        private readonly string _langCode;
        private readonly CloudService _cloudService;

        public VoucherUIController(IVoucherUIDAO voucherUIDAO, IMapper mapper, CloudService cloudService)
        {
            _voucherUIDAO = voucherUIDAO;
            _mapper = mapper;
            _langCode = "vi";
            _cloudService = cloudService;

        }

        [HttpGet]
        [Route("GetList")]
        public CommonPagination<List<VoucherUIRes>> GetList(int currentPage = 1, int recordPerPage = 10)
        {
            var res = new CommonPagination<List<VoucherUIRes>>();

            var voucherUIs = _voucherUIDAO.GetListVoucherUI(currentPage, recordPerPage, out int totalRecord, out int response);

            res.Data = _mapper.Map<List<VoucherUIRes>>(voucherUIs);
            res.TotalRecord = totalRecord;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpGet]
        [Route("GetById")]
        public CommonResponse<VoucherUIRes> GetById(Guid id)
        {
            var res = new CommonResponse<VoucherUIRes>();

            var voucherUI = _voucherUIDAO.GetVoucherUIById(id, out int response);

            if (voucherUI != null)
            {
                res.Data = _mapper.Map<VoucherUIRes>(voucherUI);
            }

            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<CommonResponse<dynamic>> Create([FromForm] VoucherUIReq request)
        {
            var res = new CommonResponse<dynamic>();

            var voucherUIDAL = _mapper.Map<VoucherUIDAL>(request);
            voucherUIDAL.CreatedAt = DateTime.Now;

            if (request.Photo != null)
            {
                // gán ImageUrl = ảnh cloud
                voucherUIDAL.ImageUrl = await _cloudService.UploadImageAsync(request.Photo).ConfigureAwait(false);
            }

            _voucherUIDAO.CreateVoucherUI(voucherUIDAL, out int response);

            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpPost]
        [Route("Update")]
        public async Task<CommonResponse<dynamic>> Update(Guid id, [FromForm] VoucherUIReq request)
        {
            var res = new CommonResponse<dynamic>();

            var voucherUIDAL = _mapper.Map<VoucherUIDAL>(request);
            voucherUIDAL.Id = id;
            voucherUIDAL.UpdatedAt = DateTime.Now;

            if (request.Photo != null)
            {
                // gán ImageUrl = ảnh cloud
                voucherUIDAL.ImageUrl = await _cloudService.UploadImageAsync(request.Photo).ConfigureAwait(false);
            }

            _voucherUIDAO.UpdateVoucherUI(voucherUIDAL, out int response);

            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpPost]
        [Route("Delete")]
        public CommonResponse<string> Delete(Guid id)
        {
            var res = new CommonResponse<string>();

            _voucherUIDAO.DeleteVoucherUI(id, out int response);

            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }
    }
}

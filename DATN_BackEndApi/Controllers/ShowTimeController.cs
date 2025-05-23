﻿using AutoMapper;
using DATN_BackEndApi.Extension;
using DATN_Helpers.Common;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.ShowTime;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Movies.Res;
using DATN_Models.DTOS.Order.Req;
using DATN_Models.DTOS.Order.Res;
using DATN_Models.DTOS.ShowTime.Req;
using DATN_Models.DTOS.ShowTime.Res;
using DATN_Services.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowTimeController : ControllerBase
    {
        private readonly IShowTimeDAO _showTimeDAO;
        private readonly IMapper _mapper;

        private readonly string _langCode;

        public ShowTimeController(IShowTimeDAO showTimeDAO, IMapper mapper, IConfiguration configuration)
        {
            _showTimeDAO = showTimeDAO;
            _mapper = mapper;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
        }


        [HttpGet]
        [Route("GetAutoDate")]
        public async Task<CommonResponse<ShowtimeAutoDateRes>> GetAutoDate([FromQuery] ShowtimeAutoDateReq showtimereq)
        {
            var res = new CommonResponse<ShowtimeAutoDateRes>();
            var result = _showTimeDAO.AutoDateNghia(showtimereq, out int response);
            var resultMapper = _mapper.Map<ShowtimeAutoDateRes>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;

        }




        /// <summary>
        /// Lấy danh sách lịch chiếu có phân trang
        /// </summary>
        [HttpGet]
        [Route("GetList")]
        public CommonPagination<List<ShowTimeRes>> GetList(
            [FromQuery] int currentPage,
            [FromQuery] int recordPerPage)
        {
            var res = new CommonPagination<List<ShowTimeRes>>();
            var result = _showTimeDAO.GetListShowTimes(currentPage, recordPerPage, out int totalRecord, out int response);

            res.Data = _mapper.Map<List<ShowTimeRes>>(result);
            res.TotalRecord = totalRecord;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }
        /// <summary>
        /// Lấy chi tiết một lịch chiếu
        /// </summary>
        [HttpGet]
        [Route("GetById/{id}")]
        public CommonResponse<ShowTimeRes> GetById(Guid id)
        {
            var res = new CommonResponse<ShowTimeRes>();
            var result = _showTimeDAO.GetShowTimeById(id, out int response);

            res.Data = _mapper.Map<ShowTimeRes>(result);
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        /// <summary>
        /// Lấy danh sách phòng trống trong khoảng thời gian
        /// </summary>
        [HttpGet]
        [Route("GetAvailableRooms")]
        public CommonResponse<List<AvailableRoomRes>> GetAvailableRooms(
            [FromQuery] DateTime startTime,
            [FromQuery] DateTime endTime)
        {
            var res = new CommonResponse<List<AvailableRoomRes>>();
            var result = _showTimeDAO.GetAvailableRooms(startTime, endTime, out int response);

            res.Data = _mapper.Map<List<AvailableRoomRes>>(result);
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        /// <summary>
        /// Lấy danh sách khung giờ trống của một phòng trong ngày
        /// </summary>
        [HttpGet]
        [Route("GetAvailableTimes")]
        public CommonResponse<List<TimeSlotRes>> GetAvailableTimes(
            [FromQuery] Guid roomId,
            [FromQuery] DateTime date)
        {
            var res = new CommonResponse<List<TimeSlotRes>>();
            var result = _showTimeDAO.GetAvailableTimes(roomId, date, out int response);

            res.Data = _mapper.Map<List<TimeSlotRes>>(result);
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        /// <summary>
        /// Tạo mới lịch chiếu
        /// </summary>
        [HttpPost]
        [Route("Create")]
        public CommonResponse<string> Create([FromBody] ShowTimeReq request)
        {
            var res = new CommonResponse<string>();
            _showTimeDAO.CreateShowTime(request, out int response);

            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        /// <summary>
        /// Cập nhật thông tin lịch chiếu
        /// </summary>
        [HttpPost]
        [Route("Update/{id}")]
        public CommonResponse<string> Update(Guid id, [FromBody] UpdateShowTimeReq request)
        {
            var res = new CommonResponse<string>();

            try
            {
                _showTimeDAO.UpdateShowTime(id, request, out int response);
                res.ResponseCode = response;
                res.Message = MessageUtils.GetMessage(response, _langCode);
                return res;
            }
            catch (Exception ex)
            {
                res.ResponseCode = -99;
                res.Message = "An error occurred while processing your request";
                return res;
            }
        }

        /// <summary>
        /// Xóa lịch chiếu
        /// </summary>
        [HttpPost]
        [Route("Delete/{id}")]
        public CommonResponse<string> Delete(Guid id)
        {
            var res = new CommonResponse<string>();
            _showTimeDAO.DeleteShowTime(id, out int response);

            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpPost]
        [Route("UpdateShowTimeStatus")]
        public CommonResponse<string> UpdateShowTimeStatus(Guid id, int status)
        {
            var res = new CommonResponse<string>();
            _showTimeDAO.UpdateShowTimeStatus(id, status, out int response);

            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }













        [HttpPost]
        [Route("ShowtimeCronjob")]
        public void ShowtimeCronjob()
        {
            _showTimeDAO.ShowtimeCronjob();
        }










    }
}
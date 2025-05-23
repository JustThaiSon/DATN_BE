﻿using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAO.Interface;
using DATN_Models.DAO.Interface.SeatAbout;
using DATN_Models.DTOS.Statistic.Res;
using DATN_Models.HandleData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DATN_Models.DTOS.Statistic.Res.StatisticRes;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly IStatisticDAO _statisticDAO;
        private readonly DATN_Context _db;

        public StatisticController(IConfiguration configuration, IUltil ultils, IMapper mapper, IStatisticDAO statisticDAO)
        {
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _statisticDAO = statisticDAO;
            _db = new DATN_Context();
        }




        [HttpGet("GetSeatProfitability")]
        public CommonResponse<List<StatisticSeatProfitabilityRes>> GetSeatProfitability(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var res = new CommonResponse<List<StatisticSeatProfitabilityRes>>();
            var result = _statisticDAO.GetSeatProfitability(startDate, endDate, out int response);

            res.Data = _mapper.Map<List<StatisticSeatProfitabilityRes>>(result);
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpGet("GetSeatOccupancy")]
        public CommonResponse<List<StatisticSeatOccupancyRes>> GetSeatOccupancy(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var res = new CommonResponse<List<StatisticSeatOccupancyRes>>();
            var result = _statisticDAO.GetSeatOccupancy(startDate, endDate, out int response);

            res.Data = _mapper.Map<List<StatisticSeatOccupancyRes>>(result);
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpGet("GetRevenueByTime")]
        public CommonResponse<List<StatisticRevenueByTimeRes>> GetRevenueByTime(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var res = new CommonResponse<List<StatisticRevenueByTimeRes>>();
            var result = _statisticDAO.GetRevenueByTime(startDate, endDate, out int response);

            res.Data = _mapper.Map<List<StatisticRevenueByTimeRes>>(result);
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpGet("GetRevenueByCinema")]
        public CommonResponse<List<StatisticRevenueByCinemaRes>> GetRevenueByCinema(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var res = new CommonResponse<List<StatisticRevenueByCinemaRes>>();
            var result = _statisticDAO.GetRevenueByCinema(startDate, endDate, out int response);

            res.Data = _mapper.Map<List<StatisticRevenueByCinemaRes>>(result);
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpGet("GetPopularGenres")]
        public CommonResponse<List<StatisticPopularGenresRes>> GetPopularGenres(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var res = new CommonResponse<List<StatisticPopularGenresRes>>();
            var result = _statisticDAO.GetPopularGenres(startDate, endDate, out int response);

            res.Data = _mapper.Map<List<StatisticPopularGenresRes>>(result);
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpGet("GetPeakHours")]
        public CommonResponse<List<StatisticPeakHoursRes>> GetPeakHours(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var res = new CommonResponse<List<StatisticPeakHoursRes>>();
            var result = _statisticDAO.GetPeakHours(startDate, endDate, out int response);

            res.Data = _mapper.Map<List<StatisticPeakHoursRes>>(result);
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpGet("GetCustomerGender")]
        public CommonResponse<List<StatisticCustomerGenderRes>> GetCustomerGender(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var res = new CommonResponse<List<StatisticCustomerGenderRes>>();
            var result = _statisticDAO.GetCustomerGender(startDate, endDate, out int response);

            res.Data = _mapper.Map<List<StatisticCustomerGenderRes>>(result);
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }





        [HttpGet]
        [Route("GetSummary_DateRange")]
        public async Task<CommonPagination<List<Statistic_SummaryDetailRes>>> GetSummary_DateRange(DateTime? Start, DateTime? End)
        {
            var res = new CommonPagination<List<Statistic_SummaryDetailRes>>();
            var result = _statisticDAO.Summary_DateRange(Start, End, out int response);
            var resultMapper = _mapper.Map<List<Statistic_SummaryDetailRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }



        [HttpGet]
        [Route("GetMovieSummary_DateRange")]
        public async Task<CommonPagination<List<Statistic_MovieDetailRes>>> GetMovieSummary_DateRange(DateTime? Start, DateTime? End)
        {
            var res = new CommonPagination<List<Statistic_MovieDetailRes>>();
            var result = _statisticDAO.Movie_DateRange(Start, End, out int response);
            var resultMapper = _mapper.Map<List<Statistic_MovieDetailRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpGet]
        [Route("GetServiceSummary_DateRange")]
        public async Task<CommonPagination<List<Statistic_ServiceDetailRes>>> GetServiceSummary_DateRange(DateTime? Start, DateTime? End)
        {
            var res = new CommonPagination<List<Statistic_ServiceDetailRes>>();
            var result = _statisticDAO.Service_DateRange(Start, End, out int response);
            var resultMapper = _mapper.Map<List<Statistic_ServiceDetailRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpGet]
        [Route("GetSummary_DateRange_Detail")]
        public CommonResponse<List<Statistic_SummaryDetailRes>> GetSummary_DateRange_Detail(DateTime? Start, DateTime? End)
        {
            var res = new CommonResponse<List<Statistic_SummaryDetailRes>>();
            var result = _statisticDAO.Summary_DateRange_Detail(Start, End, out int response);
            var resultMapper = _mapper.Map<List<Statistic_SummaryDetailRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }

        [HttpGet]
        [Route("GetRevenueDetail")]
        public async Task<CommonPagination<List<StatisticRevenueDetailRes>>> GetRevenueDetail(Guid? CinemasID, DateTime? Start, DateTime? End)
        {
            var res = new CommonPagination<List<StatisticRevenueDetailRes>>();
            var result = _statisticDAO.GetRevenueDetail(CinemasID, Start, End, out int response);
            var resultMapper = _mapper.Map<List<StatisticRevenueDetailRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }
    }
}

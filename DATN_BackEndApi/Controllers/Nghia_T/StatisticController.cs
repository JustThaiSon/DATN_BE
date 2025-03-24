using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Statistic.Res;
using Microsoft.AspNetCore.Mvc;
//using OfficeOpenXml;

namespace DATN_BackEndApi.Controllers.Test
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticDAO _statisticDAO;

        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;

        public StatisticController(IStatisticDAO statisticDAO, IConfiguration configuration, IUltil ultils, IMapper mapper)
        {
            _statisticDAO = statisticDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
        }


        #region Membership_Nghia
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
        [Route("GetMovie_DateRange")]
        public async Task<CommonPagination<List<Statistic_MovieDetailRes>>> GetMovie_DateRange(Guid MovieID, DateTime? Start, DateTime? End)
        {
            var res = new CommonPagination<List<Statistic_MovieDetailRes>>();
            var result = _statisticDAO.Movie_DateRange(MovieID, Start, End, out int response);
            var resultMapper = _mapper.Map<List<Statistic_MovieDetailRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;

            return res;
        }


        //[HttpGet]
        //[Route("Excel_Export_MOVIE_TEST")]
        //public IActionResult Excel_Export_MOVIE_TEST(Guid MovieID, DateTime? Start, DateTime? End)
        //{
        //    var result = _statisticDAO.Movie_DateRange(MovieID, Start, End, out int response);
        //    var resultMapper = _mapper.Map<List<Statistic_MovieDetailRes>>(result);

        //    // Tạo file Excel
        //    using (var package = new ExcelPackage())
        //    {
        //        var worksheet = package.Workbook.Worksheets.Add("MovieDetails");

        //        // Thêm tiêu đề vào các cột
        //        worksheet.Cells[1, 1].Value = "Date";
        //        worksheet.Cells[1, 2].Value = "Movie Name";
        //        worksheet.Cells[1, 3].Value = "Total Revenue";
        //        worksheet.Cells[1, 4].Value = "Total Tickets";
        //        worksheet.Cells[1, 5].Value = "Total Services";

        //        // Thêm dữ liệu vào các hàng trong Excel
        //        int row = 2;
        //        foreach (var item in resultMapper)
        //        {
        //            worksheet.Cells[row, 1].Value = item.Date.ToString("yyyy-MM-dd");
        //            worksheet.Cells[row, 2].Value = item.MovieName;
        //            worksheet.Cells[row, 3].Value = item.TotalRevenue;
        //            worksheet.Cells[row, 4].Value = item.TotalTickets;
        //            worksheet.Cells[row, 5].Value = item.TotalServices;
        //            row++;
        //        }

        //        var fileContents = package.GetAsByteArray();

        //        return File(fileContents,
        //                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                    "MovieDetails.xlsx");
        //    }
        //}


        #endregion


    }
}
using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Cinemas;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Cinemas.Req;
using DATN_Models.DTOS.Cinemas.Res;
using DATN_Models.DTOS.Room.Res;
using DATN_Models.HandleData;
using DATN_Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemasController : ControllerBase
    {
        private readonly ICinemasDAO _cinemasDAO;
        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;
        private readonly DATN_Context _db;

        public CinemasController(ICinemasDAO cinemaDAO, IConfiguration configuration, IUltil ultils, IMapper mapper)
        {
            _cinemasDAO = cinemaDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
            _db = new DATN_Context();
        }

        [HttpPost]
        [Route("CreateCinemas")]

        public async Task<CommonResponse<dynamic>> CreateCinemas(CinemasReq rq)
        {
            var res = new CommonResponse<dynamic>();

            var reqmapper = _mapper.Map<CinemasDAL>(rq);
            _cinemasDAO.CreateCinemas(reqmapper, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }


        [HttpPost]
        [Route("DeleteCinema")]
        public async Task<CommonResponse<dynamic>> DeleteCinema(Guid id)
        {
            try
            {
                // Sử dụng stored procedure để kiểm tra và xóa cinema
                _cinemasDAO.DeleteCinema(id, out int response);

                return new CommonResponse<dynamic>
                {
                    ResponseCode = response,
                    Message = MessageUtils.GetMessage(response, _langCode)
                };
            }
            catch (Exception ex)
            {
                return new CommonResponse<dynamic>
                {
                    ResponseCode = -101,
                    Message = $"Có lỗi xảy ra: {ex.Message}"
                };
            }
        }

        [HttpPost]
        [Route("UpdateCinemas")]

        public async Task<CommonResponse<dynamic>> UpdateCinemas(Guid IdCinemasReq, CinemasReq rq)
        {
            var res = new CommonResponse<dynamic>();
            _cinemasDAO.UpdateCinemas(IdCinemasReq, rq, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }

        [HttpGet]
        [Route("GetListCinemas")]

        public async Task<CommonPagination<List<CinemasRes>>> GetListCinemas(int currentPage, int recordPerPage)
        {

            var res = new CommonPagination<List<CinemasRes>>();

            var result = _cinemasDAO.GetListCinemas(currentPage, recordPerPage, out int TotalRecord, out int response);

            var resultMapper = _mapper.Map<List<CinemasRes>>(result);

            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = TotalRecord;

            return res;
        }

        [HttpGet]
        [Route("GetListCinemasByName")]
        public async Task<CommonPagination<List<CinemasRes>>> GetListCinemasByName(string nameCinemas, int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<CinemasRes>>();
            var result = _cinemasDAO.GetListCinemasByName(nameCinemas, currentPage, recordPerPage, out int totalRecord, out int response);
            var resultMapper = _mapper.Map<List<CinemasRes>>(result);
            res.Data = resultMapper;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            res.TotalRecord = totalRecord;

            return res;
        }


        [HttpPost]
        [Route("UpdateCinemasAddress")]

        public async Task<CommonPagination<List<CinemasRes>>> UpdateCinemasAddress(Guid IdCinemasReq, string newAddress)
        {
            var res = new CommonPagination<List<CinemasRes>>();
            _cinemasDAO.UpdateCinemasAdress(IdCinemasReq, newAddress, out int response);
            res.Data = null;
            res.Message = MessageUtils.GetMessage(response, _langCode);
            res.ResponseCode = response;
            return res;
        }


        [HttpGet]
        [Route("GetCinemaById")]
        public async Task<CommonPagination<CinemasRes>> GetCinemaById(Guid IdCinemasReq)
        {
            var res = new CommonPagination<CinemasRes>();
            var result = _cinemasDAO.GetCinemaById(IdCinemasReq, out int response);
            var resultMapper = _mapper.Map<CinemasRes>(result);
            res.Data = resultMapper;  // Trả về kết quả duy nhất
            res.Message = MessageUtils.GetMessage(response, _langCode);  // Lấy thông điệp từ mã phản hồi
            res.ResponseCode = response;  // Trả về mã phản hồi
            return res;
        }




    }
}

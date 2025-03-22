using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Genre;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Genre.Req;
using DATN_Models.DTOS.Genre.Res;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DATN_BackEndApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreDAO _genreDAO;
        private readonly string _langCode;
        private readonly IMapper _mapper;

        public GenreController(IGenreDAO genreDAO, IConfiguration configuration, IMapper mapper)
        {
            _genreDAO = genreDAO;
            _langCode = configuration.GetValue<string>("DefaultLanguageCode") ?? "vi";
            _mapper = mapper;
        }

        #region Genre CRUD
        [HttpGet]
        [Route("GetGenreList")]
        public CommonPagination<List<GetGenreRes>> GetGenreList(int currentPage, int recordPerPage)
        {
            var res = new CommonPagination<List<GetGenreRes>>();

            var result = _genreDAO.GetListGenre(currentPage, recordPerPage, out int totalRecord, out int response);
            var resultMapper = _mapper.Map<List<GetGenreRes>>(result);

            res.Data = resultMapper;
            res.TotalRecord = totalRecord;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpGet]
        [Route("GetGenreById")]
        public CommonResponse<GetGenreRes> GetGenreById(Guid id)
        {
            var res = new CommonResponse<GetGenreRes>();

            var result = _genreDAO.GetGenreById(id, out int response);
            var resultMapper = _mapper.Map<GetGenreRes>(result);

            res.Data = resultMapper;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpPost]
        [Route("CreateGenre")]
        public CommonResponse<dynamic> CreateGenre(AddGenreReq request)
        {
            var res = new CommonResponse<dynamic>();

            var reqMapper = _mapper.Map<AddGenreDAL>(request);
            _genreDAO.CreateGenre(reqMapper, out int response);

            res.Data = null;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpPost]
        [Route("UpdateGenre")]
        public CommonResponse<dynamic> UpdateGenre(Guid id, UpdateGenreReq request)
        {
            var res = new CommonResponse<dynamic>();

            var reqMapper = _mapper.Map<UpdateGenreDAL>(request);
            _genreDAO.UpdateGenre(id, reqMapper, out int response);

            res.Data = null;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }

        [HttpPost]
        [Route("DeleteGenre")]
        public CommonResponse<dynamic> DeleteGenre(Guid id)
        {
            var res = new CommonResponse<dynamic>();

            _genreDAO.DeleteGenre(id, out int response);

            res.Data = null;
            res.ResponseCode = response;
            res.Message = MessageUtils.GetMessage(response, _langCode);

            return res;
        }
        #endregion
    }
}
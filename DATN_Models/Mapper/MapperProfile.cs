using AutoMapper;
using DATN_Models.DAL.Account;
using DATN_Models.DAL.Movie;
using DATN_Models.DAL.Movie.Actor;
using DATN_Models.DAL.Rating;
using DATN_Models.DTOS.Account.Req;
using DATN_Models.DTOS.Actor;
using DATN_Models.DTOS.Movies.Req.Movie;
using DATN_Models.DTOS.Movies.Res;
using DATN_Models.DTOS.Rating.Req;
using DATN_Models.DTOS.Rating.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<CreateAccountReq, CreateAccountDAL>();

            // Phần movie
            #region Nghia_Movie

            #region Movie
            // Cái này là list movie (hiện tại chưa show hết được danh sách actor trong movie)
            CreateMap<MovieDAL, GetMovieRes>().ReverseMap();
            CreateMap<AddMovieDAL, AddMovieReq>().ReverseMap();
            #endregion

            #region Actor
            CreateMap<ListActorDAL, GetListActorRes>().ReverseMap();
            CreateMap<AddActorDAL, AddActorReq>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore()) // ko map IformFile Photo => string PhotoURL
                .ReverseMap();

            CreateMap<UpdateActorDAL, UpdateActorReq>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore()) // ko map IformFile Photo => string PhotoURL
                .ReverseMap();

            #endregion

            #endregion


            // Phần rating
            #region Nghia_Rating

            CreateMap<AddRatingDAL, AddRatingReq>().ReverseMap();
            CreateMap<UpdateRatingDAL, UpdateRatingReq>().ReverseMap();

            CreateMap<GetListRatingDAL, GetListRatingRes>().ReverseMap();


            #endregion
        }
    }
}

using AutoMapper;
using DATN_Models.DAL.Account;
using DATN_Models.DAL.Movie;
using DATN_Models.DAL.Movie.Actor;
using DATN_Models.DTOS.Account.Req;
using DATN_Models.DTOS.Movies.Req;
using DATN_Models.DTOS.Movies.Req.Actor;
using DATN_Models.DTOS.Movies.Req.Movie;
using DATN_Models.DTOS.Movies.Res;

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

        }
    }
}

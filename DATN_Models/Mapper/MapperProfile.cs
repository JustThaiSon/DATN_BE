using AutoMapper;
using DATN_Models.DAL.Account;
using DATN_Models.DAL.Movie;
using DATN_Models.DTOS.Account.Req;
using DATN_Models.DTOS.Movies.Res;

namespace DATN_Models.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<CreateAccountReq, CreateAccountDAL>();
            CreateMap<ListActorDAL, GetListActorRes>().ReverseMap();

        }
    }
}

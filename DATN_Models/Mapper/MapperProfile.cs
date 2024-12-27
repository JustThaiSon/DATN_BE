using AutoMapper;
using DATN_Models.DAL.Account;
using DATN_Models.DAL.Cinemas;
using DATN_Models.DAL.Movie;
using DATN_Models.DAL.ShowTime;
using DATN_Models.DAO;
using DATN_Models.DTOS.Account.Req;
using DATN_Models.DTOS.Cinemas.Res;
using DATN_Models.DTOS.Movies.Res;
using DATN_Models.DTOS.ShowTime.Res;
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
            CreateMap<ListActorDAL, GetListActorRes>().ReverseMap();
            CreateMap<CinemasDAL, CinemasRes>().ReverseMap();
            CreateMap<ShowTimeDAL, ShowTimeRes>().ReverseMap();
        }
    }
}

using AutoMapper;
using DATN_Models.DAL.Account;
using DATN_Models.DAL.Movie;
using DATN_Models.DAL.Room;
using DATN_Models.DTOS.Account.Req;
using DATN_Models.DTOS.Movies.Res;
using DATN_Models.DTOS.Room.Req;
using DATN_Models.DTOS.Room.Res;
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

            CreateMap<CreateRoomRes, CreateAccountDAL>();
            CreateMap<ListActorDAL, GetListActorRes>().ReverseMap();

            CreateMap<CreateRoomReq, CreateRoomDAL>().ReverseMap();
            CreateMap<ListRoomDAL, GetListRoomRes>().ReverseMap();


        }


    }
}

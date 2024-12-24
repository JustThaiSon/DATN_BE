using AutoMapper;
using DATN_Models.DAL.Account;
using DATN_Models.DAL.Movie;
using DATN_Models.DAL.Movie.Actor;
using DATN_Models.DAL.PricingRule;
using DATN_Models.DAL.Room;
using DATN_Models.DAL.Seat;
using DATN_Models.DAL.SeatType;
using DATN_Models.DTOS.Account.Req;
using DATN_Models.DTOS.Movies.Req;
using DATN_Models.DTOS.Movies.Req.Actor;
using DATN_Models.DTOS.Movies.Req.Movie;
using DATN_Models.DTOS.Movies.Res;
using DATN_Models.DTOS.PricingRule.Req;
using DATN_Models.DTOS.PricingRule.Res;
using DATN_Models.DTOS.Room.Req;
using DATN_Models.DTOS.Room.Res;
using DATN_Models.DTOS.Seat.Req;
using DATN_Models.DTOS.Seat.Res;
using DATN_Models.DTOS.SeatType.Req;
using DATN_Models.DTOS.SeatType.Res;
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
            CreateMap<CreateAccountDAL, CreateAccountReq>().ReverseMap();
            CreateMap<ListActorDAL, GetListActorRes>().ReverseMap();
            CreateMap<AddActorDAL, AddActorReq>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore()) // ko map IformFile Photo => string PhotoURL
                .ReverseMap();

            CreateMap<UpdateActorDAL, UpdateActorReq>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore()) // ko map IformFile Photo => string PhotoURL
                .ReverseMap();

            #endregion

            #endregion
            #region ThaoDepTrai
            #region Room
            CreateMap<CreateRoomReq, CreateRoomDAL>().ReverseMap();
            CreateMap<ListRoomDAL, GetListRoomRes>().ReverseMap();
            #endregion

            #region Seat
            CreateMap<GetListSeatRes, ListSeatDAL>().ReverseMap();
            CreateMap<UpdateSeatStatusDAL, UpdateSeatStatusReq>().ReverseMap();
            CreateMap<UpdateSeatTypeDAL, UpdateSeatTypeReq>().ReverseMap();
            #endregion

            #region SeatType
            CreateMap<GetListSeatTypeDAL, GetListSeatTypeRes>().ReverseMap();
            CreateMap<CreateSeatTypeDAL, CreateSeatTypeReq>().ReverseMap();
            CreateMap<UpdateSeatTypeMultiplierDAL, UpdateSeatTypeMultiplierReq>().ReverseMap();
            #endregion

            #region PricingRule
            CreateMap<CreatePricingRuleReq, CreatePricingRuleDAL>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.StartTime) ? (TimeSpan?)null : TimeSpan.Parse(src.StartTime)))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.EndTime) ? (TimeSpan?)null : TimeSpan.Parse(src.EndTime)))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.StartDate) ? (DateTime?)null : DateTime.Parse(src.StartDate)))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.EndDate) ? (DateTime?)null : DateTime.Parse(src.EndDate)))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Date) ? (DateTime?)null : DateTime.Parse(src.Date)))
                .ReverseMap();
            CreateMap<UpdatePricingRuleDAL, UpdatePricingRuleReq>().ReverseMap();
            CreateMap<GetListPricingRuleDAL, GetListPricingRuleRes>().ReverseMap();
            #endregion

            #endregion
        }


    }
}

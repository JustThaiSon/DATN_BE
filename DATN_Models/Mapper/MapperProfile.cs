using AutoMapper;
using DATN_Models.DAL.Account;
using DATN_Models.DAL.Customer;
using DATN_Models.DAL.Membership;
using DATN_Models.DAL.Movie;
using DATN_Models.DAL.Movie.Actor;
using DATN_Models.DAL.Orders;
using DATN_Models.DAL.PricingRule;
using DATN_Models.DAL.Rating;
using DATN_Models.DAL.Room;
using DATN_Models.DAL.Seat;
using DATN_Models.DAL.SeatType;
using DATN_Models.DTOS.Account.Req;
using DATN_Models.DTOS.Actor;
using DATN_Models.DTOS.Comments.Req;
using DATN_Models.DTOS.Comments.Res;
using DATN_Models.DTOS.Customer.Req;
using DATN_Models.DTOS.Customer.Res;
using DATN_Models.DTOS.Movies.Req.Movie;
using DATN_Models.DTOS.Movies.Res;
using DATN_Models.DTOS.PricingRule.Req;
using DATN_Models.DTOS.PricingRule.Res;
using DATN_Models.DTOS.Order.Req;
using DATN_Models.DTOS.Rating.Req;
using DATN_Models.DTOS.Rating.Res;
using DATN_Models.DTOS.Room.Req;
using DATN_Models.DTOS.Room.Res;
using DATN_Models.DTOS.Seat.Req;
using DATN_Models.DTOS.Seat.Res;
using DATN_Models.DTOS.SeatType.Req;
using DATN_Models.DTOS.SeatType.Res;
using DATN_Models.DTOS.Service.Request;
using DATN_Models.DAL.Service;
using DATN_Models.DTOS.Service.Response;

namespace DATN_Models.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            CreateMap<CreateAccountReq, CreateAccountDAL>();
            CreateMap<CreateAccountDAL, CreateAccountReq>().ReverseMap();

            // Phần movie
            #region Nghia_Movie
            // Cái này là list movie (hiện tại Đã SHOW ĐƯỢC được danh sách actor trong movie)
            CreateMap<MovieDAL, GetMovieRes>().ReverseMap();
            CreateMap<AddMovieDAL, AddMovieReq>()
                .ForMember(dest => dest.Thumbnail, opt => opt.Ignore()) // ko map iformfile thumnail => string thumnailURL
                .ForMember(dest => dest.Banner, opt => opt.Ignore())    // ko map iformfile banner => string bannerURL
                .ForMember(dest => dest.Trailer, opt => opt.Ignore())   // ko map iformfile trailer => string trailerURL
                .ReverseMap();
            CreateMap<UpdateMovieDAL, UpdateMovieReq>()
                .ForMember(dest => dest.Thumbnail, opt => opt.Ignore()) // ko map iformfile thumnail => string thumnailURL
                .ForMember(dest => dest.Banner, opt => opt.Ignore())    // ko map iformfile banner => string bannerURL
                .ForMember(dest => dest.Trailer, opt => opt.Ignore())   // ko map iformfile trailer => string trailerURL
                .ReverseMap();

            #endregion


            // Phần actor
            #region Nghia_Actor
            CreateMap<ListActorDAL, GetListActorRes>().ReverseMap();
            CreateMap<CreateCommentDAL, AddActorReq>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore()) // ko map IformFile Photo => string PhotoURL
                .ReverseMap();

            CreateMap<UpdateActorDAL, UpdateActorReq>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore()) // ko map IformFile Photo => string PhotoURL
                .ReverseMap();

            #endregion


            // Phần Comment
            #region Nghia_Comment
            CreateMap<CreateCommentDAL, CreateCommentReq>().ReverseMap();
            CreateMap<UpdateCommentDAL, CreateAccountDAL>().ReverseMap();

            CreateMap<ListCommentDAL, GetListCommentRes>().ReverseMap();
            //CreateMap<ListCommentDALTest, GetListCommentResTest>().ReverseMap();

            #endregion


            // Phần rating
            #region Nghia_Rating

            CreateMap<AddRatingDAL, AddRatingReq>().ReverseMap();
            CreateMap<UpdateRatingDAL, UpdateRatingReq>().ReverseMap();

            CreateMap<GetListRatingDAL, GetListRatingRes>().ReverseMap();


            #endregion


            // Phần Customer
            #region Nghia_Customer
            CreateMap<GetListCustomerInformationDAL, GetListCustomerInformationRes>().ReverseMap();
            CreateMap<UpdateCustomerDAL, UpdateCustomerReq>().ReverseMap();

            #endregion


            // Phần Membership
            #region Nghia_Membership
            CreateMap<MembershipDAL, MembershipRes>().ReverseMap();
            CreateMap<UpdateMembershipDAL, UpdateMembershipReq>().ReverseMap();
            CreateMap<AddMembershipDAL, AddMembershipReq>().ReverseMap();

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

            // Phần Order
            #region
            CreateMap<CreateOrderReq, CreateOrderDAL>().ReverseMap();
            CreateMap<TicketDAL, TicketReq>().ReverseMap();
            CreateMap<CreateTicketDAL, CreateTicketReq>().ReverseMap();
            CreateMap<CreateOrderServiceDAL, CreateOrderServiceReq>().ReverseMap();
            CreateMap<CreateServiceReq, CreateServiceDAL>().ReverseMap();
            CreateMap<UpdateServiceDAL, UpdateServiceReq>().ReverseMap();
            CreateMap<GetServiceDAL, GetServiceRes>().ReverseMap();
            CreateMap<DeleteServiceDAL, DeleteServiceReq>().ReverseMap();
            #endregion
            #region SeatbyShowTime
            CreateMap<GetListSeatByShowTimeRes, ListSeatByShowTimeDAL>().ReverseMap();
            CreateMap<UpdateSeatByShowTimeStatusDAL, UpdateSeatByShowTimeStatusReq>().ReverseMap();
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

            CreateMap<SignInDAL, SignInReq>().ReverseMap();

           
        }
    }
}

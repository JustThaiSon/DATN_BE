using AutoMapper;
using DATN_Models.DAL.Account;
using DATN_Models.DAL.AgeRating;
using DATN_Models.DAL.Cinemas;
using DATN_Models.DAL.Comment;
using DATN_Models.DAL.Customer;
using DATN_Models.DAL.Employee;
using DATN_Models.DAL.Genre;
using DATN_Models.DAL.Membership;
using DATN_Models.DAL.Movie;
using DATN_Models.DAL.Movie.Actor;
using DATN_Models.DAL.Orders;
using DATN_Models.DAL.PricingRule;
using DATN_Models.DAL.Rating;
using DATN_Models.DAL.Room;
using DATN_Models.DAL.RoomType;
using DATN_Models.DAL.Seat;
using DATN_Models.DAL.SeatType;
using DATN_Models.DAL.Service;
using DATN_Models.DAL.ServiceType;
using DATN_Models.DAL.ShowTime;
using DATN_Models.DAL.Statistic;
using DATN_Models.DAL.Voucher;
using DATN_Models.DTOS.Account.Req;
using DATN_Models.DTOS.Account.Res;
using DATN_Models.DTOS.Actor;
using DATN_Models.DTOS.AgeRating.Req;
using DATN_Models.DTOS.AgeRating.Res;
using DATN_Models.DTOS.Cinemas.Req;
using DATN_Models.DTOS.Cinemas.Res;
using DATN_Models.DTOS.Comments.Req;
using DATN_Models.DTOS.Comments.Res;
using DATN_Models.DTOS.Customer.Req;
using DATN_Models.DTOS.Customer.Res;
using DATN_Models.DTOS.Employee.Req;
using DATN_Models.DTOS.Employee.Res;
using DATN_Models.DTOS.Genre.Req;
using DATN_Models.DTOS.Genre.Res;
using DATN_Models.DTOS.Logs.Res;
using DATN_Models.DTOS.Membership.Res;
using DATN_Models.DTOS.Movies.Req.Movie;
using DATN_Models.DTOS.Movies.Res;
using DATN_Models.DTOS.Order.Req;
using DATN_Models.DTOS.Order.Res;
using DATN_Models.DTOS.PricingRule.Req;
using DATN_Models.DTOS.PricingRule.Res;
using DATN_Models.DTOS.Rating.Req;
using DATN_Models.DTOS.Rating.Res;
using DATN_Models.DTOS.Room.Req;
using DATN_Models.DTOS.Room.Res;
using DATN_Models.DTOS.RoomType.Res;
using DATN_Models.DTOS.Seat.Req;
using DATN_Models.DTOS.Seat.Res;
using DATN_Models.DTOS.SeatType.Req;
using DATN_Models.DTOS.SeatType.Res;
using DATN_Models.DTOS.Service.Request;
using DATN_Models.DTOS.Service.Response;
using DATN_Models.DTOS.ServiceType.Req;
using DATN_Models.DTOS.ServiceType.Res;
using DATN_Models.DTOS.ShowTime.Req;
using DATN_Models.DTOS.ShowTime.Res;
using DATN_Models.DTOS.Statistic.Res;
using DATN_Models.DTOS.Voucher.Req;
using DATN_Models.DTOS.Voucher.Res;
using DATN_Models.Models;
using static DATN_Models.DTOS.Statistic.Res.StatisticRes;

namespace DATN_Models.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region Account
            CreateMap<CreateAccountReq, CreateAccountDAL>();
            CreateMap<GetUserInfoDAL, GetUserInfoRes>().ReverseMap();
            CreateMap<SignInDAL, SignInReq>().ReverseMap();
            #endregion

            #region Movie
            CreateMap<MovieDAL, GetMovieRes>()
                .ForMember(dest => dest.Formats, opt => opt.MapFrom(src => src.Formats))
                .ReverseMap();
            CreateMap<AddMovieDAL, AddMovieReq>()
                .ForMember(dest => dest.Thumbnail, opt => opt.Ignore())
                .ForMember(dest => dest.Banner, opt => opt.Ignore())
                .ForMember(dest => dest.Trailer, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<UpdateMovieDAL, UpdateMovieReq>()
                .ForMember(dest => dest.Thumbnail, opt => opt.Ignore())
                .ForMember(dest => dest.Banner, opt => opt.Ignore())
                .ForMember(dest => dest.Trailer, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<GetMovieLandingDAL, GetMovieLandingRes>().ReverseMap();
            CreateMap<ListActorLangdingDAL, ListActorRes>().ReverseMap();
            CreateMap<ListGenreLangdingDAL, ListGenreRes>().ReverseMap();
            CreateMap<GetDetailMovieLangdingDAL, GetDetailMovieLangdingRes>().ReverseMap();
            CreateMap<GetShowTimeLandingDAL, GetShowTimeLangdingRes>().ReverseMap();
            CreateMap<ShowtimesLangdingDAL, ShowtimesLangdingRes>().ReverseMap();
            CreateMap<MovieGenreDAL, MovieGenreRes>().ReverseMap();
            CreateMap<GetAllNameMovieDAL, GetAllNameMovieRes>().ReverseMap();
            #endregion

            #region Actor
            CreateMap<ListActorDAL, GetListActorRes>().ReverseMap();
            CreateMap<CreateCommentDAL, AddActorReq>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<UpdateActorDAL, UpdateActorReq>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .ReverseMap();
            #endregion

            #region Comment
            CreateMap<CreateCommentDAL, CreateCommentReq>().ReverseMap();
            CreateMap<UpdateCommentDAL, CreateAccountDAL>().ReverseMap();
            CreateMap<ListCommentDAL, GetListCommentRes>().ReverseMap();
            #endregion

            #region Rating
            CreateMap<AddRatingDAL, AddRatingReq>().ReverseMap();
            CreateMap<UpdateRatingDAL, UpdateRatingReq>().ReverseMap();
            CreateMap<GetListRatingDAL, GetListRatingRes>().ReverseMap();
            #endregion

            #region Customer
            CreateMap<GetListCustomerInformationDAL, GetListCustomerInformationRes>().ReverseMap();
            CreateMap<UpdateCustomerDAL, UpdateCustomerReq>().ReverseMap();
            #endregion

            #region Employee
            CreateMap<CreateEmployeeReq, CreateEmployeeDAL>().ReverseMap();
            CreateMap<UpdateEmployeeReq, UpdateEmployeeDAL>().ReverseMap();
            CreateMap<EmployeeDAL, EmployeeRes>().ReverseMap();
            CreateMap<CheckRefundRes, CheckRefundDAL>().ReverseMap();
            CreateMap<GetmembershipByUserDAL, GetmembershipByUserRes>().ReverseMap();
            CreateMap<MembershipBenefitRes, MembershipBenefitDAL>().ReverseMap();
            CreateMap<UserMembershipDetailsDAL, UserMembershipDetailsRes>().ReverseMap();
            #endregion

            #region Voucher
            CreateMap<VoucherDAL, VoucherRes>().ReverseMap();
            CreateMap<VoucherDAL, VoucherReq>().ReverseMap();
            CreateMap<VoucherUsageDAL, VoucherUsageRes>().ReverseMap();
            CreateMap<VoucherUsageDAL, UseVoucherReq>().ReverseMap();

            // UserVoucher mappings
            CreateMap<UserVoucherDAL, UserVoucherRes>().ReverseMap();
            CreateMap<UserVoucherDAL, ClaimVoucherReq>().ReverseMap();
            CreateMap<VoucherDAL, AvailableVoucherRes>().ReverseMap();

            // VoucherUI mappings
            CreateMap<VoucherUIDAL, VoucherUIRes>().ReverseMap();
            CreateMap<VoucherUIDAL, VoucherUIReq>()
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .ReverseMap();
            #endregion

            #region Genre
            CreateMap<GenreDAL, GetGenreRes>().ReverseMap();
            CreateMap<AddGenreDAL, AddGenreReq>().ReverseMap();
            CreateMap<UpdateGenreDAL, UpdateGenreReq>().ReverseMap();
            #endregion

            #region ServiceType
            CreateMap<ServiceTypeDAL, ServiceTypeRes>().ReverseMap();
            CreateMap<CreateServiceTypeReq, CreateServiceTypeDAL>().ReverseMap();
            CreateMap<UpdateServiceTypeReq, UpdateServiceTypeDAL>().ReverseMap();
            CreateMap<GetListSeatTypeRes, GetListSeatTypeDAL>().ReverseMap();
            CreateMap<GetMovieByShowTimeRes, GetMovieByShowTimeDAL>().ReverseMap();
            CreateMap<GetPaymentDAL, GetPaymentRes>().ReverseMap();
            #endregion

            #region Cinema
            CreateMap<CinemasDAL, CinemasRes>().ReverseMap();
            CreateMap<CinemasReq, CinemasDAL>().ReverseMap();
            #endregion

            #region Room
            CreateMap<CreateRoomReq, CreateRoomDAL>().ReverseMap();
            CreateMap<ListRoomDAL, GetListRoomRes>().ReverseMap();
            CreateMap<ListRoomByCinemaDAL, GetListRoomByCinemaRes>().ReverseMap();
            CreateMap<UpdateRoomDAL, UpdateRoomReq>().ReverseMap();
            CreateMap<GetListRoomTypeDAL, RoomTypeGetListRes>().ReverseMap();
            #endregion

            #region Seat
            CreateMap<GetListSeatRes, ListSeatDAL>().ReverseMap();
            CreateMap<UpdateSeatStatusDAL, UpdateSeatStatusReq>().ReverseMap();
            CreateMap<UpdateSeatTypeDAL, UpdateSeatTypeReq>().ReverseMap();
            CreateMap<SetupPair, SetupPairReq>().ReverseMap();
            CreateMap<GetListSeatByShowTimeRes, ListSeatByShowTimeDAL>().ReverseMap();
            CreateMap<UpdateSeatByShowTimeStatusDAL, UpdateSeatByShowTimeStatusReq>().ReverseMap();
            CreateMap<GetSeatByShowTimeRes, GetSeatByShowTimeDAL>().ReverseMap();
            #endregion

            #region SeatType
            CreateMap<GetListSeatTypeDAL, GetListSeatTypeRes>().ReverseMap();
            CreateMap<CreateSeatTypeDAL, CreateSeatTypeReq>().ReverseMap();
            CreateMap<UpdateSeatTypeMultiplierDAL, UpdateSeatTypeMultiplierReq>().ReverseMap();
            #endregion






            // ==========================================================
            #region Order
            CreateMap<TicketDAL, TicketReq>().ReverseMap();
            CreateMap<CreateOrderServiceDAL, CreateOrderServiceReq>().ReverseMap();
            CreateMap<CreateServiceReq, CreateServiceDAL>().ReverseMap();
            CreateMap<UpdateServiceDAL, UpdateServiceReq>().ReverseMap();
            CreateMap<GetServiceDAL, GetServiceRes>().ReverseMap();
            CreateMap<DeleteServiceDAL, DeleteServiceReq>().ReverseMap();
            CreateMap<GetListTicketDAL, GetListTicketRes>().ReverseMap();
            CreateMap<GetDetailOrderDAL, GetDetailOrderRes>().ReverseMap();
            CreateMap<GetStatusByIdDAL, GetStatusByIdRes>().ReverseMap();
            CreateMap<CreateOrderReq, CreateOrderDAL>().ReverseMap();
            CreateMap<ServiceReq, ServiceDAL>().ReverseMap();
            CreateMap<CheckMemberShipRes, CheckMemberShipDAL>().ReverseMap();
            CreateMap<MembershipPreviewDAL, MembershipPreviewRes>().ReverseMap();
            CreateMap<GetOrderDetailLangdingRes, GetOrderDetailLangdingDAL>().ReverseMap();
            #endregion
            // ==========================================================






            #region PricingRule
            CreateMap<CreatePricingRuleReq, CreatePricingRuleDAL>().ReverseMap();
            CreateMap<UpdatePricingRuleDAL, UpdatePricingRuleReq>().ReverseMap();
            CreateMap<GetListPricingRuleDAL, GetListPricingRuleRes>().ReverseMap();
            #endregion

            #region Statistic
            CreateMap<Statistic_MovieDetailDAL, Statistic_MovieDetailRes>().ReverseMap();
            CreateMap<Statistic_SummaryDetailDAL, Statistic_SummaryDetailRes>().ReverseMap();
            CreateMap<StatisticRevenueDetailDAL, StatisticRevenueDetailRes>().ReverseMap();
            CreateMap<ChangeLog, GetLogRes>().ReverseMap();
            CreateMap<StatisticSeatProfitabilityDAL, StatisticSeatProfitabilityRes>().ReverseMap();
            CreateMap<StatisticSeatOccupancyDAL, StatisticSeatOccupancyRes>().ReverseMap();
            CreateMap<StatisticRevenueByTimeDAL, StatisticRevenueByTimeRes>().ReverseMap();
            CreateMap<StatisticRevenueByCinemaDAL, StatisticRevenueByCinemaRes>()
                .ForMember(dest => dest.TotalRevenue, opt => opt.MapFrom(src => src.TotalRevenue));
            CreateMap<StatisticPopularGenresDAL, StatisticPopularGenresRes>().ReverseMap();
            CreateMap<StatisticPeakHoursDAL, StatisticPeakHoursRes>().ReverseMap();
            CreateMap<StatisticCustomerGenderDAL, StatisticCustomerGenderRes>().ReverseMap();
            CreateMap<Statistic_ServiceDetailDAL, Statistic_ServiceDetailRes>().ReverseMap();
            #endregion

            #region ShowTime
            CreateMap<ShowTime, ShowTimeDAL>().ReverseMap();
            CreateMap<ShowTimeDAL, ShowTimeRes>().ReverseMap();
            CreateMap<AvailableRoomDAL, AvailableRoomRes>().ReverseMap();
            CreateMap<TimeSlotDAL, TimeSlotRes>()
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable == 1))
                .ReverseMap()
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable ? 1 : 0));
            CreateMap<ShowTimeReq, ShowTimeDAL>();
            CreateMap<ShowTimeReq, ShowTime>();
            CreateMap<ShowtimeAutoDateDAL, ShowtimeAutoDateRes>().ReverseMap();
            #endregion

            #region AgeRating
            CreateMap<AgeRatingDAL, AgeRatingRes>().ReverseMap();
            CreateMap<CreateAgeRatingReq, AgeRatingDAL>().ReverseMap();
            CreateMap<UpdateAgeRatingReq, AgeRatingDAL>().ReverseMap();
            #endregion

            #region MovieFormat
            CreateMap<DATN_Models.DAL.MovieFormat.MovieFormatDAL, DATN_Models.DTOS.MovieFormat.Res.MovieFormatRes>().ReverseMap();
            CreateMap<DATN_Models.DTOS.MovieFormat.Req.CreateMovieFormatReq, DATN_Models.DAL.MovieFormat.MovieFormatDAL>().ReverseMap();
            CreateMap<DATN_Models.DTOS.MovieFormat.Req.UpdateMovieFormatReq, DATN_Models.DAL.MovieFormat.MovieFormatDAL>().ReverseMap();
            CreateMap<DATN_Models.DAL.MovieFormat.MovieFormatMovieDAL, DATN_Models.DTOS.MovieFormat.Res.MovieFormatMovieRes>().ReverseMap();
            CreateMap<DATN_Models.DTOS.MovieFormat.Req.AssignFormatToMovieReq, DATN_Models.DAL.MovieFormat.MovieFormatMovieDAL>().ReverseMap();
            CreateMap<DATN_Models.DAL.Movie.MovieFormatInfoDAL, DATN_Models.DTOS.Movies.Res.MovieFormatInfoRes>().ReverseMap();
            #endregion
        }
    }
}

using AutoMapper;
using DATN_Models.DAL.Account;
using DATN_Models.DAL.Cinemas;
using DATN_Models.DAL.Comment;
using DATN_Models.DAL.Counter;
using DATN_Models.DAL.Customer;
using DATN_Models.DAL.Employee;
using DATN_Models.DAL.Employee;
using DATN_Models.DAL.Genre;
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
using DATN_Models.DTOS.Actor;
using DATN_Models.DTOS.Cinemas.Req;
using DATN_Models.DTOS.Cinemas.Res;
using DATN_Models.DTOS.Comments.Req;
using DATN_Models.DTOS.Comments.Res;
using DATN_Models.DTOS.Counter.Res;
using DATN_Models.DTOS.Customer.Req;
using DATN_Models.DTOS.Customer.Res;
using DATN_Models.DTOS.Employee.Req;
using DATN_Models.DTOS.Employee.Req;
using DATN_Models.DTOS.Employee.Res;
using DATN_Models.DTOS.Employee.Res;
using DATN_Models.DTOS.Genre.Req;
using DATN_Models.DTOS.Genre.Res;
using DATN_Models.DTOS.Logs.Res;
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

            CreateMap<CreateAccountReq, CreateAccountDAL>();
            CreateMap<GetUserInfoDAL, GetUserInfoRes>().ReverseMap();
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
            CreateMap<GetMovieLandingDAL, GetMovieLandingRes>().ReverseMap();
            CreateMap<ListActorLangdingDAL, ListActorRes>().ReverseMap();
            CreateMap<ListGenreLangdingDAL, ListGenreRes>().ReverseMap();
            CreateMap<GetDetailMovieLangdingDAL, GetDetailMovieLangdingRes>().ReverseMap();
            CreateMap<GetShowTimeLandingDAL, GetShowTimeLangdingRes>().ReverseMap();
            CreateMap<ShowtimesLangdingDAL, ShowtimesLangdingRes>().ReverseMap();

            CreateMap<MovieGenreDAL, MovieGenreRes>().ReverseMap();
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

     

            // Phần nhân viên
            #region Nghia_Employee
            CreateMap<CreateEmployeeReq, CreateEmployeeDAL>().ReverseMap();
            CreateMap<UpdateEmployeeReq, UpdateEmployeeDAL>().ReverseMap();
            CreateMap<EmployeeDAL, EmployeeRes>().ReverseMap();
            CreateMap<CheckRefundRes, CheckRefundDAL>().ReverseMap();
            //CreateMap<ChangePasswordReq, ChangePasswordReq>().ReverseMap();
            #endregion

            // Phần voucher
            #region Nghia_Voucher
            CreateMap<VoucherDAL, VoucherRes>().ReverseMap();
            CreateMap<VoucherDAL, VoucherReq>().ReverseMap();
            CreateMap<VoucherUsageDAL, VoucherUsageRes>().ReverseMap();
            CreateMap<VoucherUsageDAL, UseVoucherReq>().ReverseMap();
            #endregion

            // Phần thể loại
            #region Nghia_Genre

            CreateMap<GenreDAL, GetGenreRes>().ReverseMap();
            CreateMap<AddGenreDAL, AddGenreReq>().ReverseMap();
            CreateMap<UpdateGenreDAL, UpdateGenreReq>().ReverseMap();

            #endregion

            // Phần dịch vụ
            #region Nghia_Service_Type
            CreateMap<ServiceTypeDAL, ServiceTypeRes>().ReverseMap();
            CreateMap<CreateServiceTypeReq, CreateServiceTypeDAL>().ReverseMap();
            CreateMap<UpdateServiceTypeReq, UpdateServiceTypeDAL>().ReverseMap();
            CreateMap<GetListSeatTypeRes, GetListSeatTypeDAL>().ReverseMap();
            CreateMap<GetMovieByShowTimeRes, GetMovieByShowTimeDAL>().ReverseMap();
            CreateMap<GetPaymentDAL, GetPaymentRes>().ReverseMap();
            #endregion




            #region Cinema
            CreateMap<CinemasDAL, CinemasRes>().ReverseMap();


            #endregion



            #region Cinema
            CreateMap<CinemasDAL, CinemasRes>().ReverseMap();


            #endregion


            // Phần Thống kê
            #region Nghia_Statistic
            CreateMap<Statistic_MovieDetailDAL, Statistic_MovieDetailRes>().ReverseMap();
            CreateMap<Statistic_SummaryDetailDAL, Statistic_SummaryDetailRes>().ReverseMap();

            CreateMap<ChangeLog, GetLogRes>().ReverseMap();

            #endregion




            #region Cinema
            CreateMap<CinemasDAL, CinemasRes>().ReverseMap();


            #endregion


            // Phần Thống kê
            #region Nghia_Statistic
            CreateMap<Statistic_MovieDetailDAL, Statistic_MovieDetailRes>().ReverseMap();
            CreateMap<Statistic_SummaryDetailDAL, Statistic_SummaryDetailRes>().ReverseMap();

            #endregion


            #region Showtime
            CreateMap<ShowTimeDAL, ShowTimeRes>().ReverseMap();


            #region Showtime
            CreateMap<ShowTimeDAL, ShowTimeRes>().ReverseMap();

            #endregion


            #region Showtime
            CreateMap<ShowTimeDAL, ShowTimeRes>().ReverseMap();

            #endregion

            #region ThaoDepTrai
            #region 
            CreateMap<CinemasReq, CinemasDAL>().ReverseMap();
            CreateMap<CinemasDAL, CinemasRes>().ReverseMap();
            #endregion

            #region Room
            CreateMap<CreateRoomReq, CreateRoomDAL>().ReverseMap();
            CreateMap<ListRoomDAL, GetListRoomRes>().ReverseMap();
            CreateMap<UpdateRoomDAL, UpdateRoomReq>().ReverseMap();
            #endregion
            CreateMap<GetListRoomTypeDAL, RoomTypeGetListRes>().ReverseMap();

            #region Seat
            CreateMap<GetListSeatRes, ListSeatDAL>().ReverseMap();
            CreateMap<UpdateSeatStatusDAL, UpdateSeatStatusReq>().ReverseMap();
            CreateMap<UpdateSeatTypeDAL, UpdateSeatTypeReq>().ReverseMap();
            CreateMap<SetupPair, SetupPairReq>().ReverseMap();
            #endregion

            // Phần Order
            #region
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
            CreateMap<TicketReq, TicketDAL>().ReverseMap();
            CreateMap<ServiceReq, ServiceDAL>().ReverseMap();
            CreateMap<CheckMemberShipRes, CheckMemberShipDAL>().ReverseMap();
            CreateMap<MembershipPreviewDAL, MembershipPreviewRes>().ReverseMap();
            CreateMap<GetOrderDetailLangdingRes, GetOrderDetailLangdingDAL>().ReverseMap();
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

                .ReverseMap();
            CreateMap<UpdatePricingRuleDAL, UpdatePricingRuleReq>().ReverseMap();
            CreateMap<GetListPricingRuleDAL, GetListPricingRuleRes>().ReverseMap();
            #endregion

            #endregion




            #region statistic
            CreateMap<StatisticTopServicesDAL, StatisticTopServicesRes>().ReverseMap();
            CreateMap<StatisticSeatProfitabilityDAL, StatisticSeatProfitabilityRes>().ReverseMap();
            CreateMap<StatisticSeatOccupancyDAL, StatisticSeatOccupancyRes>().ReverseMap();
            CreateMap<StatisticRevenueByTimeDAL, StatisticRevenueByTimeRes>().ReverseMap();
            CreateMap<StatisticRevenueByCinemaDAL, StatisticRevenueByCinemaRes>().ReverseMap();
            CreateMap<StatisticPopularGenresDAL, StatisticPopularGenresRes>().ReverseMap();
            CreateMap<StatisticPeakHoursDAL, StatisticPeakHoursRes>().ReverseMap();
            CreateMap<StatisticCustomerGenderDAL, StatisticCustomerGenderRes>().ReverseMap();
            CreateMap<StatisticBundledServicesDAL, StatisticBundledServicesRes>().ReverseMap();
            #endregion


            CreateMap<SignInDAL, SignInReq>().ReverseMap();
            CreateMap<GetAllNameMovieDAL, GetAllNameMovieRes>().ReverseMap();
            CreateMap<GetSeatByShowTimeRes, GetSeatByShowTimeDAL>().ReverseMap();

            #region ShowTime
            // Entity <-> DAL mappings
            CreateMap<ShowTime, ShowTimeDAL>()
                .ReverseMap();

            // DAL <-> DTO mappings
            CreateMap<ShowTimeDAL, ShowTimeRes>()
                .ReverseMap();
            CreateMap<AvailableRoomDAL, AvailableRoomRes>()
                .ReverseMap();
            CreateMap<TimeSlotDAL, TimeSlotRes>()
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable == 1))
                .ReverseMap()
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable ? 1 : 0));

            // Request mappings
            CreateMap<ShowTimeReq, ShowTimeDAL>();
            CreateMap<ShowTimeReq, ShowTime>();
            #endregion



            #endregion

            //counter

            CreateMap<Counter_NowPlayingMovies_GetList_DTO, Counter_NowPlayingMovies_GetList_DAL>().ReverseMap();


        }
    }
}

using AutoMapper;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Movies.Req.Movie;
using DATN_Models.DTOS.Movies.Res;
using Microsoft.AspNetCore.Mvc;

namespace DATN_BackEndApi.Controllers
{
    //[BAuthorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipDAO _membershipDAO;

        private readonly string _langCode;
        private readonly IUltil _ultils;
        private readonly IMapper _mapper;

        public MembershipController(IMembershipDAO membershipDAO, IConfiguration configuration, IUltil ultils, IMapper mapper)
        {
            _membershipDAO = membershipDAO;
            _langCode = configuration["MyCustomSettings:LanguageCode"] ?? "vi";
            _ultils = ultils;
            _mapper = mapper;
        }

    }
}

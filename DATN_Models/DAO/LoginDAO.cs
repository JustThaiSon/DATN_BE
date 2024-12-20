using DATN_Helpers.Constants;
using DATN_Helpers.Extensions;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Account;
using DATN_Models.DTOS.Account.Req;
using DATN_Models.DTOS.Account.Res;
using DATN_Models.HandleData;
using DATN_Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DATN_Models.DAO
{
    public class LoginDAO : ILoginDAO
    {
        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly RoleManager<AppRoles> _roleManager;
        private readonly DATN_Context _context;
        private static Dictionary<string, OtpCacheEntry> _otpCache = new Dictionary<string, OtpCacheEntry>();
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginDAO(RoleManager<AppRoles> roleManager, UserManager<AppUsers> _userManager, SignInManager<AppUsers> _signInManager, DATN_Context _context, IHttpContextAccessor httpContextAccessor)
        {
            _roleManager = roleManager;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._context = _context;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<(LoginDTO LoginDto, int Response)> login(string userName, string passWord)
        {
            int response = 0;
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                response = (int)ResponseCodeEnum.ERR_USER_NOT_FOUND;
                return (null, response);
            }

            var result = await _signInManager.PasswordSignInAsync(user, passWord, false, true);
            if (!result.Succeeded)
            {
                response = (int)ResponseCodeEnum.ERR_SYSTEM;
                return (null, response);
            }
            var roleNames = await _userManager.GetRolesAsync(user);
            var roles = _context.Roles
                      .Where(r => roleNames.Contains(r.Name))
                      .Select(x => new RoleRes
                      {
                          Name = x.Name
                      })
                      .ToList();
            List<string> roleNamesList = roles.Select(role => role.Name).ToList();
            var loginDTO = new LoginDTO
            {
                ID = user.Id,
                DisplayName = user.Name,
                UserName = user.UserName,
                Roles = roleNamesList
            };
            response = (int)ResponseCodeEnum.SUCCESS;
            return (loginDTO, response);
        }

        public async Task<(int, string)> RegisterUserAsync(CreateAccountReq request)
        {
            int response = 0;
            if (!StringExtension.IsValidEmail(request.Email))
            {
                response = (int)ResponseCodeEnum.ERR_INVALID_EMAIL;

                return (response, null);
            }

            if (!StringExtension.IsValidPhoneNumber(request.PhoneNumber))
            {
                response = (int)ResponseCodeEnum.ERR_INVALID_PHONENUMBER;
                return (response, null);
            }

            if (_context.Users.Any(x => x.Email == request.Email))
            {
                response = (int)ResponseCodeEnum.ERR_EMAIL_EXIST;
                return (response, null);
            }

            string otp = GenerateOtp();

            _otpCache[request.Email] = new OtpCacheEntry
            {
                Otp = otp,
                UserInfo = request
            };
            _httpContextAccessor.HttpContext.Session.SetString("email", request.Email);

            await SendOtpAsync(request.Email, otp);
            response = (int)ResponseCodeEnum.OTP_SENT;

            return (response, otp);
        }

        public async Task<int> VerifyOtpAndRegisterUserAsync(string email, string otp)
        {

            if (!_otpCache.ContainsKey(email))
            {
                Console.WriteLine($"Key {email} không tồn tại trong _otpCache.");
                return (int)ResponseCodeEnum.ERR_INVALID_OTP;
            }

            if (_otpCache[email].Otp != otp)
            {
                Console.WriteLine($"OTP không khớp. Nhập: {otp}, Lưu: {_otpCache[email].Otp}");
                return (int)ResponseCodeEnum.ERR_INVALID_OTP;
            }
            var userInfo = _otpCache[email].UserInfo;

            _otpCache.Remove(email);

            var newAccount = new AppUsers
            {
                UserName = userInfo.Email,
                Email = userInfo.Email,
                Name = userInfo.Name,
                Dob = userInfo.Dob,
                PhoneNumber = userInfo.PhoneNumber,
                Address = userInfo.Address,
                Sex = userInfo.Sex,
            };

            var result = await _userManager.CreateAsync(newAccount, userInfo.Password);
            if (!result.Succeeded)
                return (int)ResponseCodeEnum.ERR_SYSTEM;

            return (int)ResponseCodeEnum.SUCCESS;
        }


        private string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString(); // Tạo OTP 6 chữ số
        }


        private async Task SendOtpAsync(string email, string otp)
        {
            // Gửi email (có thể tích hợp với dịch vụ email như SendGrid, SMTP)
            // Đây là ví dụ giả lập
            Console.WriteLine($"Gửi OTP {otp} đến email {email}");
        }


    }
}

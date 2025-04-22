using DATN_Helpers.Common;
using DATN_Helpers.Constants;
using DATN_Helpers.Database;
using DATN_Helpers.Extensions;
using DATN_Models.DAL.Account;
using DATN_Models.DAL.Cinemas;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Account;
using DATN_Models.DTOS.Account.Req;
using DATN_Models.DTOS.Account.Res;
using DATN_Models.HandleData;
using DATN_Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DATN_Models.DAO
{
    public class LoginDAO : ILoginDAO
    {
        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly RoleManager<AppRoles> _roleManager;
        private readonly DATN_Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;
        private static string connectionString = string.Empty;

        public LoginDAO(RoleManager<AppRoles> roleManager, UserManager<AppUsers> _userManager, SignInManager<AppUsers> _signInManager, DATN_Context _context, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
        {
            _roleManager = roleManager;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._context = _context;
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
            var configuration = new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json")
               .Build();
            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
        }


        public async Task<(LoginDTO LoginDto, int Response)> login(SignInDAL req)
        {
            int response = 0;
            var user = await _userManager.FindByNameAsync(req.UserName);
            if (user == null)
            {
                response = (int)ResponseCodeEnum.ERR_USER_NOT_FOUND;
                return (null, response);
            }

            var result = await _signInManager.PasswordSignInAsync(user, req.PassWord, true, true);
            if (!result.Succeeded)
            {
                response = (int)ResponseCodeEnum.ERR_PASSWORD;
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
                Roles = roleNamesList,
                Email = user.Email
            };
            response = (int)ResponseCodeEnum.SUCCESS;
            SaveSession(user.Id);
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

            if (_context.Users.Any(x => x.PhoneNumber == request.PhoneNumber))
            {
                response = (int)ResponseCodeEnum.ERR_EXISTS_PHONENUMBER;
                return (response, null);
            }

            // Sinh OTP
            string otp = GenerateOtp();

            // Xóa bản ghi cũ nếu tồn tại
            var existingOtp = await _context.OptLog.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (existingOtp != null)
            {
                _context.OptLog.Remove(existingOtp);
            }

            var newOtp = new OptLog
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                OtpCode = otp,
                CreatedAt = DateTime.UtcNow
            };
            await _context.OptLog.AddAsync(newOtp);
            await _context.SaveChangesAsync();

            _memoryCache.Set(request.Email, request, TimeSpan.FromMinutes(5)); 

            response = (int)ResponseCodeEnum.SUCCESS;
            return (response, otp);
        }


        public void SaveSession(Guid userId)
        {

            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@key", SqlDbType.NVarChar) { Value = "UserId" };
                pars[1] = new SqlParameter("@value", SqlDbType.UniqueIdentifier) { Value = userId };

                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("sp_set_session_context", pars);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public async Task<int> VerifyOtpAndRegisterUserAsync(VerifyOtpReq req)
        {
            if (!_memoryCache.TryGetValue(req.Email, out CreateAccountReq userInfo))
            {
                Console.WriteLine($"Không tìm thấy thông tin người dùng trong cache cho email: {req.Email}");
                return (int)ResponseCodeEnum.ERR_INVALID_OTP;
            }

            var otpEntry = await _context.OptLog.FirstOrDefaultAsync(x => x.Email == req.Email);

            if (otpEntry == null)
            {
                Console.WriteLine($"Không tìm thấy OTP trong DB cho email: {req.Email}");
                return (int)ResponseCodeEnum.ERR_INVALID_OTP;
            }

            if (otpEntry.OtpCode != req.Otp)
            {
                Console.WriteLine($"OTP không khớp. Nhập: {req.Otp}, DB: {otpEntry.OtpCode}");
                return (int)ResponseCodeEnum.ERR_INVALID_OTP;
            }

            _context.OptLog.Remove(otpEntry);
            _memoryCache.Remove(req.Email);

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
            {
                Console.WriteLine("Tạo tài khoản thất bại: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                return (int)ResponseCodeEnum.ERR_SYSTEM;
            }

            await _context.SaveChangesAsync(); 

            return (int)ResponseCodeEnum.SUCCESS;
        }


        private string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public GetUserInfoDAL GetUserInfo(Guid userId, out int response)
        {
            response = 0;
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_UserId", userId);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<GetUserInfoDAL>("SP_User_GetUserInfor", pars);
                response = ConvertUtil.ToInt(pars[1].Value);
                return result;
            }
            catch (Exception)
            {
                response = -99;
                throw;
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public async Task<int> ChangePasswordAsync(Guid userId, ChangePasswordCustomerReq req)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return (int)ResponseCodeEnum.ERR_USER_NOT_FOUND;
            }

            var result = await _userManager.ChangePasswordAsync(user, req.CurrentPassword, req.NewPassword);
            if (!result.Succeeded)
            {
                return (int)ResponseCodeEnum.ERR_SYSTEM;
            }

            return (int)ResponseCodeEnum.SUCCESS;
        }
    }
}

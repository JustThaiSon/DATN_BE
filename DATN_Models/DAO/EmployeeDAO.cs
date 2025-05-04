using DATN_Helpers.Common;
using DATN_Helpers.Constants;
using DATN_Helpers.Database;
using DATN_Models.DAL.Employee;
using DATN_Models.DAO.Interface;
using DATN_Models.DTOS.Employee.Req;
using DATN_Models.HandleData;
using DATN_Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DATN_Models.DAO
{
    public class EmployeeDAO : IEmployeeDAO
    {
        private static string connectionString = string.Empty;
        private readonly UserManager<AppUsers> _userManager;
        private readonly RoleManager<AppRoles> _roleManager;

        public EmployeeDAO(UserManager<AppUsers> userManager, RoleManager<AppRoles> roleManager)
        {
            var configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build();

            connectionString = configuration.GetConnectionString("Db") ?? string.Empty;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<int> CreateEmployee(CreateEmployeeDAL req)
        {
            try
            {
                var user = new AppUsers
                {
                    UserName = req.UserName,
                    Email = req.Email,
                    PhoneNumber = req.PhoneNumber,
                    Name = req.Name,
                    Dob = req.Dob,
                    Sex = req.Sex,
                    Address = req.Address,
                    CreatedDate = DateTime.Now,
                    Status = 1
                };

                var result = await _userManager.CreateAsync(user, req.PasswordHash);
                if (result.Succeeded)
                {
                    // Tìm vai trò "Employee" bằng ID
                    var roleId = new Guid("280972EF-41A9-42D1-9CF4-5AE276366166");

                    // Thêm vai trò cho người dùng
                    var userRole = new IdentityUserRole<Guid>
                    {
                        UserId = user.Id,
                        RoleId = roleId
                    };

                    using (var context = new DATN_Context())
                    {
                        context.UserRoles.Add(userRole);

                        // Thêm thông tin về rạp chiếu phim mà nhân viên quản lý
                        if (req.CinemaIds != null && req.CinemaIds.Count > 0)
                        {
                            foreach (var cinemaId in req.CinemaIds)
                            {
                                var userCinema = new AppUsers_Cinemas
                                {
                                    UserId = user.Id,
                                    CinemasId = cinemaId
                                };
                                context.Set<AppUsers_Cinemas>().Add(userCinema);
                            }
                        }

                        await context.SaveChangesAsync();
                    }

                    Console.WriteLine("ĐÃ TẠO NV");
                    return (int)ResponseCodeEnum.SUCCESS; // Success
                }
                else
                {
                    Console.WriteLine("TẠO NV FAIL");
                    return (int)ResponseCodeEnum.ERR_WRONG_INPUT; // User creation failed
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the employee: {ex.Message}");
                return (int)ResponseCodeEnum.ERR_SYSTEM; // System error
            }
        }

        public async Task<int> UpdateEmployee(Guid id, UpdateEmployeeDAL req)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user != null)
                {
                    user.UserName = req.UserName;
                    user.Email = req.Email;
                    user.PhoneNumber = req.PhoneNumber;
                    user.Name = req.Name;
                    user.Dob = req.Dob;
                    user.Sex = req.Sex;
                    user.Address = req.Address;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        // Cập nhật thông tin về rạp chiếu phim mà nhân viên quản lý
                        using (var context = new DATN_Context())
                        {
                            // Xóa tất cả các liên kết hiện tại
                            var existingUserCinemas = context.Set<AppUsers_Cinemas>().Where(uc => uc.UserId == id).ToList();
                            if (existingUserCinemas.Any())
                            {
                                context.Set<AppUsers_Cinemas>().RemoveRange(existingUserCinemas);
                            }

                            // Thêm các liên kết mới
                            if (req.CinemaIds != null && req.CinemaIds.Count > 0)
                            {
                                foreach (var cinemaId in req.CinemaIds)
                                {
                                    var userCinema = new AppUsers_Cinemas
                                    {
                                        UserId = id,
                                        CinemasId = cinemaId
                                    };
                                    context.Set<AppUsers_Cinemas>().Add(userCinema);
                                }
                            }

                            await context.SaveChangesAsync();
                        }

                        return 200; // Success
                    }

                    return -99; // Update failed
                }
                else
                {
                    return -99; // User not found
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the employee.", ex);
            }
        }

        public async Task<int> DeleteEmployee(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user != null)
                {
                    user.Status = 0;
                    var result = await _userManager.UpdateAsync(user);
                    return result.Succeeded ? 200 : -99;
                }
                else
                {
                    return -99; // User not found
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the employee.", ex);
            }
        }

        public async Task<(List<EmployeeDAL>, int, int)> GetListEmployee(int currentPage, int recordPerPage)
        {
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[4];
                pars[0] = new SqlParameter("@_CurrentPage", currentPage);
                pars[1] = new SqlParameter("@_RecordPerPage", recordPerPage);
                pars[2] = new SqlParameter("@_TotalRecord", SqlDbType.Int) { Direction = ParameterDirection.Output };
                pars[3] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetListSP<EmployeeDAL>("SP_Employee_GetList", pars);
                int response = ConvertUtil.ToInt(pars[3].Value);
                int totalRecord = ConvertUtil.ToInt(pars[2].Value);

                // Lấy thông tin về các rạp chiếu phim cho mỗi nhân viên trong danh sách
                if (result != null && result.Count > 0)
                {
                    using (var context = new DATN_Context())
                    {
                        // Lấy tất cả các ID của nhân viên trong danh sách
                        var employeeIds = result.Select(e => e.Id).ToList();

                        // Lấy tất cả các liên kết giữa nhân viên và rạp chiếu phim
                        var userCinemas = context.Set<AppUsers_Cinemas>()
                            .Where(uc => employeeIds.Contains(uc.UserId))
                            .ToList();

                        if (userCinemas.Any())
                        {
                            // Lấy tất cả các ID của rạp chiếu phim
                            var cinemaIds = userCinemas.Select(uc => uc.CinemasId).Distinct().ToList();

                            // Lấy thông tin chi tiết về các rạp chiếu phim
                            var cinemas = context.Set<Cinemas>()
                                .Where(c => cinemaIds.Contains(c.CinemasId) && !c.IsDeleted)
                                .Select(c => new { c.CinemasId, c.Name, c.Address, c.PhoneNumber })
                                .ToList();

                            // Gán thông tin về rạp chiếu phim cho mỗi nhân viên
                            foreach (var employee in result)
                            {
                                var employeeCinemaIds = userCinemas
                                    .Where(uc => uc.UserId == employee.Id)
                                    .Select(uc => uc.CinemasId)
                                    .ToList();

                                employee.Cinemas = cinemas
                                    .Where(c => employeeCinemaIds.Contains(c.CinemasId))
                                    .Select(c => new CinemaInfoDAL
                                    {
                                        CinemasId = c.CinemasId,
                                        Name = c.Name,
                                        Address = c.Address,
                                        PhoneNumber = c.PhoneNumber
                                    })
                                    .ToList();
                            }
                        }
                    }
                }

                return (result, totalRecord, response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the employee list.", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }

        public async Task<(EmployeeDAL, int)> GetEmployeeDetail(Guid id)
        {
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_Id", id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                var result = db.GetInstanceSP<EmployeeDAL>("SP_Employee_GetDetail", pars);
                int response = ConvertUtil.ToInt(pars[1].Value);

                // Lấy thông tin về các rạp chiếu phim mà nhân viên đang quản lý
                using (var context = new DATN_Context())
                {
                    var userCinemas = context.Set<AppUsers_Cinemas>()
                        .Where(uc => uc.UserId == id)
                        .ToList();

                    if (userCinemas.Any())
                    {
                        var cinemaIds = userCinemas.Select(uc => uc.CinemasId).ToList();

                        // Lấy thông tin chi tiết về các rạp chiếu phim
                        var cinemas = context.Set<Cinemas>()
                            .Where(c => cinemaIds.Contains(c.CinemasId) && !c.IsDeleted)
                            .Select(c => new CinemaInfoDAL
                            {
                                CinemasId = c.CinemasId,
                                Name = c.Name,
                                Address = c.Address,
                                PhoneNumber = c.PhoneNumber
                            })
                            .ToList();

                        result.Cinemas = cinemas;
                    }
                }

                return (result, response);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the employee details.", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }


        public async Task<int> ChangePassword(ChangePasswordReq req)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(req.UserId.ToString());
                if (user == null)
                {
                    return -999999; // User not found
                }

                var result = await _userManager.ChangePasswordAsync(user, req.CurrentPassword, req.NewPassword);
                return result.Succeeded ? 200 : -999998; // Success or failure
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while changing the password.", ex);
            }
        }

        public async Task<int> ToggleLockoutEmployee(Guid id)
        {
            DBHelper db = null;
            try
            {
                var pars = new SqlParameter[2];
                pars[0] = new SqlParameter("@_EmployeeId", id);
                pars[1] = new SqlParameter("@_Response", SqlDbType.Int) { Direction = ParameterDirection.Output };
                db = new DBHelper(connectionString);
                db.ExecuteNonQuerySP("SP_Employee_LockoutToggle", pars);
                int response = ConvertUtil.ToInt(pars[1].Value);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while toggling employee lockout status.", ex);
            }
            finally
            {
                if (db != null)
                    db.Close();
            }
        }
    }
}

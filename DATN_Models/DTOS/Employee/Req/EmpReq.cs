using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Employee.Req
{
    public class CreateEmployeeReq
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public int Sex { get; set; }
        public string Address { get; set; }
    }

    public class UpdateEmployeeReq
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public int Sex { get; set; }
        public string Address { get; set; }
    }
    public class ChangePasswordReq
    {
        public Guid UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}

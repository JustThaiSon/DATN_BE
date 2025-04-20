using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Employee
{
    public class CreateEmployeeDAL
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

    public class UpdateEmployeeDAL
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public int Sex { get; set; }
        public string Address { get; set; }
    }

    public class EmployeeDAL
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public int Sex { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}

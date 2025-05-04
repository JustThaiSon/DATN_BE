using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Employee.Res
{
    public class EmployeeRes
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
        public List<CinemaInfoRes> Cinemas { get; set; } = new List<CinemaInfoRes>();
    }

    public class CinemaInfoRes
    {
        public Guid CinemasId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}

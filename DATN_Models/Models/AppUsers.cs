using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.Models
{
    public class AppUsers : IdentityUser<Guid>
    {
        public string Name { get; set; }

        public DateTime Dob { get; set; }

        public int Status { get; set; }

        public string Address { get; set; }
        public int Sex { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

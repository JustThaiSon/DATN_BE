using DATN_Helpers.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Cinema
{
    public class UpdateCinemaDAL
    {
        public Guid CinemasId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int TotalRooms { get; set; }
        public CinemaStatusEnum Status { get; set; }
    }
}

using DATN_Helpers.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Seat
{
    public class UpdateSeatStatusDAL
    {
        public Guid Id { get; set; }
        public SeatStatusEnum Status { get; set; }
    }
}

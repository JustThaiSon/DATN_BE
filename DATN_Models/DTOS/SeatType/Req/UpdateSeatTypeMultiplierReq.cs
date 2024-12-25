using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.SeatType.Req
{
    public class UpdateSeatTypeMultiplierReq
    {
        public Guid Id { get; set; }
        public long Multiplier { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.SeatType.Req
{
    public class CreateSeatTypeReq
    {
        public string SeatTypeName { get; set; }
        public long Multiplier { get; set; }
    }
}

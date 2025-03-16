using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Seat.Req
{
    public class SetupPairReq
    {
        public Guid Seatid1 { get; set; }
        public Guid Seatid2 { get; set; }
        public Guid RoomId { get; set; }
    }
}

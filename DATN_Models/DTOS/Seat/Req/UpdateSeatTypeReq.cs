using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Seat.Req
{
    public class UpdateSeatTypeReq
    {
        public Guid Id { get; set; }
        public Guid SeatTypeId { get; set; }
    }
}

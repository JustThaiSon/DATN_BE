using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Counter.Req
{
    public class TicketOrderReq
    {
        public string Email { get; set; }
        public Guid? UserId { get; set; }
        public bool IsAnonymous { get; set; } = true;
        public Guid ShowTimeId { get; set; }
        public List<SeatSelectionReq> SelectedSeats { get; set; }
        public List<ServiceSelectionReq> SelectedServices { get; set; }
    }
}

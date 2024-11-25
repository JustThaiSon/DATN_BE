using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.Entities
{
    public class Seats
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid SeaTypeId { get; set; }
        public string NameSeat { get; set; }
        public int Status { get; set; }
    }
}

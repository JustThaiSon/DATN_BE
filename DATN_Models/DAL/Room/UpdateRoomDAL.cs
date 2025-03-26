using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Room
{
    public class UpdateRoomDAL
    {
        public Guid Id { get; set; }
        public Guid RoomTypeId { get; set; }
        public string Name { get; set; }
        public long SeatPrice { get; set; }
        public int Status { get; set; }
    }
}

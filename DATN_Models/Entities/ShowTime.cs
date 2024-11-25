using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.Entities
{
    public class ShowTime
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid RoomId { get; set; }         
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}

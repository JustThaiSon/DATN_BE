using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.RoomType.Res
{
    public class RoomTypeGetListRes
    {
        public Guid RoomTypeId { get; set; }
        public string Name { get; set; }
        public int status { get; set; }
    }
}

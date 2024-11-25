using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.Entities
{
    public class MovieActors
    {
        public Guid MovieId { get; set; }
        public Guid ActorId { get; set; }
    }
}

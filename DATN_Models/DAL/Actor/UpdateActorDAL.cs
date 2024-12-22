using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Movie.Actor
{
    public class UpdateActorDAL
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Biography { get; set; }
        public string PhotoURL { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Movie.Actor
{
    public class CreateCommentDAL
    {
        public string Content { get; set; }
        public Guid MovieID { get; set; }
    }
}

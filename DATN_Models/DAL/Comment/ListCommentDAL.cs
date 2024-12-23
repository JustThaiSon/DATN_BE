using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Movie.Actor
{
    public class ListCommentDAL
    {
        public Guid Id { get; set; }
        public Guid UserID { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public string CreateDate { get; set; }
        public int Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Comments.Req
{
    public class CreateCommentReq
    {
        public string Content { get; set; }
        public Guid MovieID { get; set; }

    }
}

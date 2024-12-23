using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Rating
{
    public class GetListRatingDAL
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string MovieName { get; set; }
        public decimal RatingValue { get; set; }
        public string CreateDate { get; set; }
        public int Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DAL.Rating
{
    public class AddRatingDAL
    {
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
        public decimal RatingValue { get; set; }
    }
}

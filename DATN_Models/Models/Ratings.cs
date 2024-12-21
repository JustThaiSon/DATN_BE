using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.Models
{
    public class Ratings
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
        public decimal RatingValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
    }
}

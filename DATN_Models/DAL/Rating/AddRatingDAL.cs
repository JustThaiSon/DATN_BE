namespace DATN_Models.DAL.Rating
{
    public class AddRatingDAL
    {
        public Guid UserId { get; set; }
        public Guid MovieId { get; set; }
        public decimal RatingValue { get; set; }
    }
}

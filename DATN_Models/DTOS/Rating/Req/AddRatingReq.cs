namespace DATN_Models.DTOS.Rating.Req
{
    public class AddRatingReq
    {
        public Guid MovieId { get; set; }
        public decimal RatingValue { get; set; }
    }
}

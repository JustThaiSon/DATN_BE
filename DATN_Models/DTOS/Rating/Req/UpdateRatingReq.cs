namespace DATN_Models.DTOS.Rating.Req
{
    public class UpdateRatingReq
    {
        public Guid RatingId { get; set; }
        public decimal RatingValue { get; set; }
    }
}

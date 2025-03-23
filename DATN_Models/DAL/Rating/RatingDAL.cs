namespace DATN_Models.DAL.Rating
{
    public class RatingDAL
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public double RatingValue { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
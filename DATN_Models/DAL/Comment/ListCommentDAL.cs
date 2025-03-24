namespace DATN_Models.DAL.Comment
{
    public class ListCommentDAL
    {
        public Guid Id { get; set; }
        public Guid UserID { get; set; }
        public Guid MovieID { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public double ratingvalue { get; set; }
        public string CreateDate { get; set; }
        public int Status { get; set; }
    }
}

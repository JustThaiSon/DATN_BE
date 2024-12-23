namespace DATN_Models.DTOS.Comments.Res
{
    public class GetListCommentRes
    {
        public Guid Id { get; set; }
        public Guid UserID { get; set; }
        public string Username { get; set; }
        public string Content { get; set; }
        public string CreateDate { get; set; }
        public int Status { get; set; }

    }
}

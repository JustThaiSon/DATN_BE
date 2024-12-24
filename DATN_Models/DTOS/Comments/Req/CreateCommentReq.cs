namespace DATN_Models.DTOS.Comments.Req
{
    public class CreateCommentReq
    {
        public string Content { get; set; }
        public Guid MovieID { get; set; }

    }
}

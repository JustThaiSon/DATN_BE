namespace DATN_Models.DTOS.Account.Req
{
    public class LoginEmployeeReq
    {
        public Guid CinemaId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}

namespace DATN_Models.DTOS.Account
{
    public class LoginDTO
    {
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }

}

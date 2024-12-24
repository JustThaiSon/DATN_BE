namespace DATN_Models.DAL.Account
{
    public class CreateAccountDAL
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public DateTime Dob { get; set; }
        public int Sex { get; set; }
    }
}

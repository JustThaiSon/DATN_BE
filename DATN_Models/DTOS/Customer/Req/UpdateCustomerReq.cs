namespace DATN_Models.DTOS.Customer.Req
{
    public class UpdateCustomerReq
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public int Sex { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public int LockoutEnabled { get; set; }
    }
}

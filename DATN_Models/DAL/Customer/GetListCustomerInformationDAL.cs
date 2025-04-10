﻿namespace DATN_Models.DAL.Customer
{
    public class GetListCustomerInformationDAL
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public int Sex { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public int LockoutEnabled { get; set; }
        public int EmailConfirmed { get; set; }
        public string RoleName { get; set; }
    }
}

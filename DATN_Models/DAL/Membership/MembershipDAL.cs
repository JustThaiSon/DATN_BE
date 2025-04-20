﻿namespace DATN_Models.DAL.Membership
{
    public class MembershipDAL
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public long DurationInDays { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
    }
}

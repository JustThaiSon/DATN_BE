﻿namespace DATN_Models.DAL.OrderManagement
{
    public class OrderManagementDAL
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Email { get; set; }
        public string OrderCode { get; set; }
        public long TotalPrice { get; set; }
        public int Status { get; set; }
        public bool IsAnonymous { get; set; }
        public long DiscountPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string StatusText { get; set; }
        public string FormattedTotalPrice { get; set; }
        public string FormattedCreatedDate { get; set; }
    }
}

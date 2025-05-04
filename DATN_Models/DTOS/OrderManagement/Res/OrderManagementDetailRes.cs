﻿namespace DATN_Models.DTOS.OrderManagement.Res
{
    public class OrderManagementDetailRes
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string OrderCode { get; set; }
        public long TotalPrice { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string FormattedCreatedDate { get; set; }
        public string FormattedTotalPrice { get; set; }
        public List<OrderTicketDetailRes> Tickets { get; set; } = new List<OrderTicketDetailRes>();
        public List<OrderServiceDetailRes> Services { get; set; } = new List<OrderServiceDetailRes>();
    }

    public class OrderTicketDetailRes
    {
        public Guid Id { get; set; }
        public string TicketCode { get; set; }
        public string MovieName { get; set; }
        public string RoomName { get; set; }
        public string SeatName { get; set; }
        public string ShowTime { get; set; }
        public int Duration { get; set; }
        public string CinemaName { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
        public long Price { get; set; }
        public string FormattedPrice { get; set; }
    }

    public class OrderServiceDetailRes
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public long Price { get; set; }
        public long Quantity { get; set; }
        public long TotalPrice { get; set; }
        public string FormattedPrice { get; set; }
        public string FormattedTotalPrice { get; set; }
    }
}

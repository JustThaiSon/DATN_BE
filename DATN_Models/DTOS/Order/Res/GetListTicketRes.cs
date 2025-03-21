namespace DATN_Models.DTOS.Order.Res
{
    public class GetListTicketRes
    {
        public string TickeCode { get; set; }
        public string MovieName { get; set; }
        public string RoomNumber { get; set; }
        public string SeatName { get; set; }
        public string ShowTime { get; set; }
        public int Duration { get; set; }
        public string CinemaName { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

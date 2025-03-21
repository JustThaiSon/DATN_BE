namespace DATN_Models.DTOS.Seat.Res
{
    public class GetSeatByShowTimeRes
    {
        public Guid SeatStatusByShowTimeId { get; set; }
        public Guid SeatId { get; set; }
        public int Status { get; set; }
        public long SeatPrice { get; set; }
        public string SeatName { get; set; }
        public int RowNumber { get; set; }
        public int ColNumber { get; set; }
        public string SeatTypeName { get; set; }
        public Guid? PairId { get; set; }
    }
}

namespace DATN_Models.DTOS.Movies.Res
{
    public class ShowtimesLangdingRes
    {
        public Guid Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public Guid RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
    }
}

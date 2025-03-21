namespace DATN_Models.DTOS.Logs.Res
{
    public class GetLogRes
    {
        public long Id { get; set; }
        public Guid? UserId { get; set; } // Nullable nếu không phải lúc nào cũng có UserId
        public string Action { get; set; } // Hành động (Insert, Update, Delete)
        public string TableName { get; set; } // Tên bảng bị thay đổi
        public Guid RecordId { get; set; } // ID của bản ghi bị thay đổi
        public string BeforeChange { get; set; } // Dữ liệu trước khi thay đổi (JSON hoặc chuỗi)
        public string AfterChange { get; set; } // Dữ liệu sau khi thay đổi (JSON hoặc chuỗi)
        public DateTime ChangeDateTime { get; set; } = DateTime.Now; // Thời gian thay đổi mặc định là hiện tại
    }
}

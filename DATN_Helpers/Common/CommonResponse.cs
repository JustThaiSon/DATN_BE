using Newtonsoft.Json;

namespace DATN_Helpers.Common
{

    /// <summary>
    /// Dữ liệu phản hồi từ DB
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonResponse<T>
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    /// <summary>
    /// Phân trang dữ liệu. <br />
    /// <b>Dùng để:</b> Hiển thị lên table, slide, hoặc giao diện cần phân trang.
    /// </summary>
    /// <value>
    /// Các thông tin về phản hồi và dữ liệu phân trang:
    /// <list type="bullet">
    ///     <item><b>ResponseCode:</b> Mã phản hồi từ hệ thống (xác định kết quả yêu cầu).</item>
    ///     <item><b>Message:</b> Thông báo chi tiết dựa trên mã phản hồi.</item>
    ///     <item><b>TotalRecord:</b> Tổng số bản ghi trong bộ dữ liệu.</item>
    ///     <item><b>Data:</b> Dữ liệu trả về cho trang hiện tại (danh sách các mục trong trang).</item>
    /// </list>
    /// </value>
    /// <typeparam name="T">Đối tượng cần phân trang (diễn viên, phim, hoá đơn,...)</typeparam>
    public class CommonPagination<T>
    {
        /// <summary>
        /// Mã phản hồi
        /// </summary>
        public int ResponseCode { get; set; }

        /// <summary>
        /// Thông báo chi tiết về kết quả của req
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Dữ liệu trả về cho trang htai
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Tổng số bản ghi trong csdl
        /// </summary>
        public int TotalRecord { get; set; }
    }
    public class CommonMessage<T>
    {
        [JsonProperty("i")]
        public long MessageId { get; set; }
        [JsonProperty("m")]
        public string Method { get; set; }
        [JsonProperty("dt")]
        public T Data { get; set; }
    }

}

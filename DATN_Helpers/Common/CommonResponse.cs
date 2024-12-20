namespace DATN_Helpers.Common
{
    public class CommonResponse<T>
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
    public class CommonPagination<T>
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int TotalRecord { get; set; }
    }
}

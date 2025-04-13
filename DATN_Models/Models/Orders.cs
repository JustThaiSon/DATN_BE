namespace DATN_Models.Models
{
    public class Orders
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string OrderCode { get; set; }
        public Guid? UserId { get; set; }
        public long TotalPrice { get; set; }
        public int Status { get; set; } // 0 chưa thanh toán , 1 đã thanh toán , 2 đã sử dụng 
        public int IsAnonymous { get; set; }
        public Guid? PaymentId { get; set; }
        public Guid? VoucherId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

}

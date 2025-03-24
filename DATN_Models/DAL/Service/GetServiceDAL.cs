namespace DATN_Models.DAL.Service
{
    public class GetServiceDAL
    {
        public Guid Id { get; set; }
        public Guid ServiceTypeID { get; set; }
        public string ImageUrl { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public int Status { get; set; }
    }
}

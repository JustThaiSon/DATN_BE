namespace DATN_Models.DTOS.Service.Response
{
    public class GetServiceRes
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public string CategoryName { get; set; }
    }
}

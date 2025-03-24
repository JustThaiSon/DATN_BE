namespace DATN_Models.DTOS.ServiceType.Req
{
    public class CreateServiceTypeReq
    {
        public string Name { get; set; }
        public string Description { get; set; }
        //public string ImageUrl { get; set; }
    }


    public class UpdateServiceTypeReq
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public string ImageUrl { get; set; }
    }


    public class DeleteServiceTypeReq
    {
        public Guid Id { get; set; }
    }
}
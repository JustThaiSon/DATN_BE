using DATN_Models.DAL.Movie;
using DATN_Models.DAL.Orders;
using DATN_Models.DAL.Service;

namespace DATN_Models.DAL.ServiceType
{
    public class ServiceTypeDAL
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int Status { get; set; }
        public List<GetServiceDAL>? serviceList { get; set; }
    }


    public class CreateServiceTypeDAL
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }


    public class UpdateServiceTypeDAL
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
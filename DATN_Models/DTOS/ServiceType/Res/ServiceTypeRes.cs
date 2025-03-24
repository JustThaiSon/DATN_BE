using DATN_Models.DAL.Service;

namespace DATN_Models.DTOS.ServiceType.Res
{
    public class ServiceTypeRes
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
        //public int Status { get; set; }
        public List<GetServiceDAL>? serviceList { get; set; }
    }
}
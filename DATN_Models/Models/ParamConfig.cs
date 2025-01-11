namespace DATN_Models.Models
{
    public class ParamConfig
    {
        public long Id { get; set; }
        public int GroupConfigId { get; set; }
        public string ParamType { get; set; }
        public string ParamCode { get; set; }
        public string ParamValue { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }

    }
}

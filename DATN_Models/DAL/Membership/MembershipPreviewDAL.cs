namespace DATN_Models.DAL.Membership
{
    public class MembershipPreviewDAL
    {
        public long DiscountAmount { get; set; }
        public int PointWillEarn { get; set; }
        public List<string> FreeService { get; set; }
        public string FreeServiceString { get; set; }
        public void ParseFreeServiceString()
        {
            if (!string.IsNullOrEmpty(FreeServiceString))
            {
                FreeService = FreeServiceString.Split(',')
                                                .Select(s => s.Trim()) 
                                                .ToList();
            }
        }
    }
}

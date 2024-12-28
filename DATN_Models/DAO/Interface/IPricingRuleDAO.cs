using DATN_Models.DAL.PricingRule;

namespace DATN_Models.DAO.Interface
{
    public interface IPricingRuleDAO
    {
        List<GetListPricingRuleDAL> GetListPricingRule(int currentPage, int recordPerPage, out int totalRecord, out int response);
        void UpdatePricingRule(UpdatePricingRuleDAL dataInput, out int response);
        void CreatePricingRule(CreatePricingRuleDAL dataInput, out int response);
        void DeletePricingRule(Guid dataInput, out int response);
    }
}

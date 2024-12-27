using DATN_Models.DAL.PricingRule;
using DATN_Models.DAL.SeatType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

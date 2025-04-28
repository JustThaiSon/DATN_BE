﻿using DATN_Models.DAL.MembershipBenefit;

namespace DATN_Models.DAO.Interface
{
    public interface IMembershipBenefitDAO
    {
        List<MembershipBenefitDAL> GetAllMembershipBenefits(out int response);
        List<MembershipBenefitDAL> GetMembershipBenefitsByMembershipId(long membershipId, out int response);
        MembershipBenefitDAL GetMembershipBenefitById(long id, out int response);
        void CreateMembershipBenefit(MembershipBenefitDAL membershipBenefit, out int response);
        void UpdateMembershipBenefit(MembershipBenefitDAL membershipBenefit, out int response);
        void DeleteMembershipBenefit(long id, out int response);
    }
}

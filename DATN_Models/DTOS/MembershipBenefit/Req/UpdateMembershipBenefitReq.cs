﻿using Microsoft.AspNetCore.Http;

namespace DATN_Models.DTOS.MembershipBenefit.Req
{
    public class UpdateMembershipBenefitReq : MembershipBenefitReq
    {
        public long Id { get; set; }
    }
}

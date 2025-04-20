namespace DATN_Models.DTOS.Membership.Req
{
    public class AddUserMembershipReq
    {
        public long MembershipId { get; set; }
        public Guid? PaymentMethodId { get; set; }
    }
}

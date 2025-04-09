using DATN_Models.DTOS.Membership.Req;

namespace DATN_Models.DAO.Interface
{
    public interface IMembershipDAO
    {
        void AddUserMembership(Guid userId,AddUserMembershipReq userMembership,out int response);
    }
}

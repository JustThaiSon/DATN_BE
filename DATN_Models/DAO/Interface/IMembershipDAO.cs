using DATN_Models.DAL.Membership;
using DATN_Models.DTOS.Membership.Req;

namespace DATN_Models.DAO.Interface
{
    public interface IMembershipDAO
    {
        void AddUserMembership(Guid userId,AddUserMembershipReq userMembership,out int response);
        CheckMemberShipDAL CheckMembership(Guid userId, out int response);
        MembershipPreviewDAL MembershipPreview(Guid userId,long orderPrice,long ticketPrice, out int response);
        GetmembershipByUserDAL GetmembershipByUser(Guid userId, out int response);
    }
}

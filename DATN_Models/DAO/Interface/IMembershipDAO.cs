using DATN_Models.DAL.Membership;
using DATN_Models.DTOS.Membership.Req;
using DATN_Models.DTOS.Membership.Res;

namespace DATN_Models.DAO.Interface
{
    public interface IMembershipDAO
    {
        void AddUserMembership(Guid userId,AddUserMembershipReq userMembership,out int response);
        CheckMemberShipDAL CheckMembership(Guid userId, out int response);
        MembershipPreviewDAL MembershipPreview(Guid userId,long orderPrice,long ticketPrice, out int response);
        GetmembershipByUserDAL GetmembershipByUser(Guid userId, out int response);
        GetPointByUserRes GetPointByUser(Guid userId, out int response);
        List<GetPointHistoryRes> GetPointHistory(Guid userId,int type, int currentPage, int recordPerPage, out int totalRecord, out int response);
        List<MembershipDAL> GetAllMemberships(out int response);
    }
}

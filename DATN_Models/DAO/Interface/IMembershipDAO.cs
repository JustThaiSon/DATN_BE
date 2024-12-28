using DATN_Models.DAL.Membership;

namespace DATN_Models.DAO.Interface
{
    public interface IMembershipDAO
    {
        #region Membership_nghia
        void CreateMembership(AddMembershipDAL req, out int response);
        void UpdateMembership(UpdateMembershipDAL req, out int response);
        void DeleteMembership(Guid Id, out int response);
        List<MembershipDAL> GetListMembership(int currentPage, int recordPerPage, out int totalRecord, out int response);
        MembershipDAL GetMembershipDetail(Guid Id, out int response);

        #endregion
    }
}

using DATN_Models.DAL.Voucher;
using System;
using System.Collections.Generic;

namespace DATN_Models.DAO.Interface
{
    public interface IUserVoucherDAO
    {
        // Người dùng nhận voucher
        void ClaimVoucher(UserVoucherDAL req, out int response);

        // Lấy danh sách voucher của người dùng
        List<UserVoucherDAL> GetUserVouchers(Guid userId, int currentPage, int recordPerPage, out int totalRecord, out int response);

        // Lấy chi tiết một voucher của người dùng
        UserVoucherDAL GetUserVoucherById(Guid id, out int response);

        // Cập nhật trạng thái voucher của người dùng (đã sử dụng, hết hạn, hủy)
        void UpdateUserVoucherStatus(Guid id, int status, out int response);



        // Lấy danh sách voucher có thể nhận
        List<VoucherDAL> GetAvailableVouchers(int currentPage, int recordPerPage, out int totalRecord, out int response);

        // Tăng số lượng voucher của người dùng
        void IncreaseUserVoucherQuantity(Guid userId, Guid voucherId, int quantity, out int response);
    }
}

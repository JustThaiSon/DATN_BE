using DATN_Models.DAL.Voucher;
using System;
using System.Collections.Generic;

namespace DATN_Models.DAO.Interface
{
    public interface IVoucherDAO
    {
        void CreateVoucher(VoucherDAL req, out int response);
        void UpdateVoucher(VoucherDAL req, out int response);
        void DeleteVoucher(Guid id, out int response);
        VoucherDAL GetVoucherById(Guid id, out int response);
        VoucherDAL GetVoucherByCode(string code, out int response);
        List<VoucherDAL> GetListVoucher(int currentPage, int recordPerPage, out int totalRecord, out int response);
        void UseVoucher(VoucherUsageDAL req, out int response);
        List<VoucherUsageDAL> GetVoucherUsageHistory(Guid voucherId, int currentPage, int recordPerPage, out int totalRecord, out int response);
    }
}
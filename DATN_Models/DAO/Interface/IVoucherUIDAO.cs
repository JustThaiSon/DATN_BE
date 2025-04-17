﻿using DATN_Models.DAL.Voucher;
using System;
using System.Collections.Generic;

namespace DATN_Models.DAO.Interface
{
    public interface IVoucherUIDAO
    {
        void CreateVoucherUI(VoucherUIDAL req, out int response);
        void UpdateVoucherUI(VoucherUIDAL req, out int response);
        void DeleteVoucherUI(Guid id, out int response);
        VoucherUIDAL GetVoucherUIById(Guid id, out int response);
        List<VoucherUIDAL> GetListVoucherUI(int currentPage, int recordPerPage, out int totalRecord, out int response);
    }
}

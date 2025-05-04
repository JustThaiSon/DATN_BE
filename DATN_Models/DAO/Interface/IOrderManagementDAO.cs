﻿using DATN_Models.DAL.OrderManagement;

namespace DATN_Models.DAO.Interface
{
    public interface IOrderManagementDAO
    {
        List<OrderManagementDAL> GetList(DateTime? fromDate, DateTime? toDate, int currentPage, int recordPerPage, out int totalRecord, out int response);
        OrderManagementDetailDAL GetDetail(Guid orderId, out int response);
    }
}

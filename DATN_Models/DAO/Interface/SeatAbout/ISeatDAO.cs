﻿using DATN_Models.DAL.Seat;

namespace DATN_Models.DAO.Interface.SeatAbout
{
    public interface ISeatDAO
    {
        List<ListSeatDAL> GetListSeat(Guid id, int currentPage, int recordPerPage, out int totalRecord, out int response);
        List<ListSeatByShowTimeDAL> GetListSeatByShowTime(Guid showTimeId, int currentPage, int recordPerPage, out int totalRecord, out int response);
        void UpdateSeatStatus(UpdateSeatStatusDAL dataInput, out int response);
        void UpdateSeatByShowTimeStatus(UpdateSeatByShowTimeStatusDAL dataInput, out int response);
        void UpdateSeatType(UpdateSeatTypeDAL dataInput, out int response);
        GetStatusByIdDAL GetStatusById(Guid Id, out int response);
        void SetupPair(SetupPair dataINput, out int response);
        List<GetSeatByShowTimeDAL> GetSeatByShowTime(Guid showTimeId, out int totalRecord, out int response);

    }
}

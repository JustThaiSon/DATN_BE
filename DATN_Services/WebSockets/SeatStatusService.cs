using DATN_Helpers.Constants;
using DATN_Models.DAL.Seat;
using DATN_Models.DAO.Interface.SeatAbout;
using DATN_Models.DTOS.Entities;
using DATN_Models.DTOS.Room.Req;
using DATN_Models.Models;
using System;
using System.Collections.Concurrent;
using System.Globalization;

namespace DATN_Services.WebSockets
{
    public class SeatStatusService
    {
        private readonly ConcurrentDictionary<Guid, SeatStatusByShow> _seats = new();
        private readonly Dictionary<string, List<string>> _userHeldSeats = new();
        private readonly ISeatDAO _seatDAO;
        public SeatStatusService(ISeatDAO seatDAO)
        {
            _seatDAO = seatDAO;
        }

        /// <summary>
        /// Thêm hoặc cập nhật trạng thái ghế.
        /// </summary>
        public bool AddOrUpdateSeatStatus(Guid seatId, int status)
        {
            if (seatId == Guid.Empty) return false;

            _seats.AddOrUpdate(
                seatId,
                new SeatStatusByShow
                {
                    Id = seatId,
                    Status = status
                },
                (key, existingSeat) =>
                {
                    existingSeat.Status = status;
                    return existingSeat;
                }
            );
            return true;
        }

        /// <summary>
        /// Lấy trạng thái ghế theo ID.
        /// </summary>
        public SeatStatusByShow? GetSeatStatus(Guid seatId)
        {
            if (seatId == Guid.Empty) return null; // Tránh lỗi ID rỗng

            _seats.TryGetValue(seatId, out var seat);
            return seat;
        }

        /// <summary>
        /// Xóa ghế khỏi danh sách.
        /// </summary>
        public bool RemoveSeat(Guid seatId)
        {
            if (seatId == Guid.Empty) return false; // Tránh lỗi ID rỗng

            return _seats.TryRemove(seatId, out _);
        }

        /// <summary>
        /// Lấy danh sách tất cả ghế.
        /// </summary>
        public List<SeatStatusByShow> GetAllSeats()
        {
            return _seats.Values.ToList();
        }

        /// <summary>
        /// Generate seats for a given room.
        /// </summary>
        public List<GetSeatByShowTimeDAL> GenerateSeats(Guid showtimeId)
        {
            var seats = _seatDAO.GetSeatByShowTime(showtimeId, out int totalRecord, out int responseCode);
            return seats;
        }

        public void HoldSeatForUser(string userId, string seatId)
        {
            if (!_userHeldSeats.ContainsKey(userId))
                _userHeldSeats[userId] = new List<string>();

            _userHeldSeats[userId].Add(seatId);
        }

        public List<string> GetHeldSeatsByUser(string userId)
        {
            return _userHeldSeats.ContainsKey(userId) ? _userHeldSeats[userId] : new List<string>();
        }
    }

    public class SeatStatusByShow
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
    }

}
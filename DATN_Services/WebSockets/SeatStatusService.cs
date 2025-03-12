using DATN_Helpers.Constants;
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

        /// <summary>
        /// Thêm hoặc cập nhật trạng thái ghế.
        /// </summary>
        public bool AddOrUpdateSeatStatus(Guid seatId, SeatStatusEnum status)
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
        public List<SeatInfo> GenerateSeats(Guid roomId)
        {
            var seats = new List<SeatInfo>
                    {
                        new SeatInfo { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Row = "A", Number = 1, Type = "standard", Price = 140000, Status = GetSeatStatus(Guid.Parse("11111111-1111-1111-1111-111111111111"))?.Status ?? SeatStatusEnum.Available, PairSeatId = null },
                        new SeatInfo { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Row = "A", Number = 2, Type = "standard", Price = 140000, Status = GetSeatStatus(Guid.Parse("22222222-2222-2222-2222-222222222222"))?.Status ?? SeatStatusEnum.Available, PairSeatId = null },
                        new SeatInfo { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Row = "B", Number = 1, Type = "vip", Price = 180000, Status = GetSeatStatus(Guid.Parse("33333333-3333-3333-3333-333333333333"))?.Status ?? SeatStatusEnum.Available, PairSeatId = null },
                        new SeatInfo { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Row = "B", Number = 2, Type = "vip", Price = 180000, Status = GetSeatStatus(Guid.Parse("44444444-4444-4444-4444-444444444444"))?.Status ?? SeatStatusEnum.Available, PairSeatId = null },
                        new SeatInfo { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Row = "C", Number = 1, Type = "vip", Price = 180000, Status = GetSeatStatus(Guid.Parse("55555555-5555-5555-5555-555555555555"))?.Status ?? SeatStatusEnum.Available, PairSeatId = null },
                        new SeatInfo { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Row = "C", Number = 2, Type = "vip", Price = 180000, Status = GetSeatStatus(Guid.Parse("66666666-6666-6666-6666-666666666666"))?.Status ?? SeatStatusEnum.Available, PairSeatId = null },
                        new SeatInfo { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), Row = "G", Number = 1, Type = "couple", Price = 160000, Status = GetSeatStatus(Guid.Parse("77777777-7777-7777-7777-777777777777"))?.Status ?? SeatStatusEnum.Available, PairSeatId = "G2" },
                        new SeatInfo { Id = Guid.Parse("88888888-8888-8888-8888-888888888888"), Row = "G", Number = 2, Type = "couple", Price = 160000, Status = GetSeatStatus(Guid.Parse("88888888-8888-8888-8888-888888888888"))?.Status ?? SeatStatusEnum.Available, PairSeatId = "G1" },
                        new SeatInfo { Id = Guid.Parse("99999999-9999-9999-9999-999999999999"), Row = "H", Number = 1, Type = "couple", Price = 160000, Status = GetSeatStatus(Guid.Parse("99999999-9999-9999-9999-999999999999"))?.Status ?? SeatStatusEnum.Available, PairSeatId = "H2" },
                        new SeatInfo { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Row = "H", Number = 2, Type = "couple", Price = 160000, Status = GetSeatStatus(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"))?.Status ?? SeatStatusEnum.Available, PairSeatId = "H1" }
                    };

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
        public SeatStatusEnum Status { get; set; }
    }

}
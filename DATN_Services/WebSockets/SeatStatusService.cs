using DATN_Helpers.Constants;
using DATN_Models.DTOS.Entities;
using DATN_Models.Models;
using System.Collections.Concurrent;

namespace DATN_Services.WebSockets
{
    public class SeatStatusService
    {
        private readonly ConcurrentDictionary<Guid, SeatStatusByShow> _seats = new();
        public void AddOrUpdateSeatStatus(Guid seatId, SeatStatusEnum status)
        {
            _seats.AddOrUpdate(
                seatId,
                new SeatStatusByShow { Id = seatId, Status = status },
                (key, existingSeat) =>
                {
                    existingSeat.Status = status;
                    return existingSeat;
                }
            );
        }
        public SeatStatusByShow GetSeatStatus(Guid seatId)
        {
            _seats.TryGetValue(seatId, out var seat);
            return seat;
        }
        public void RemoveSeat(Guid seatId)
        {
            _seats.TryRemove(seatId, out _);
        }

    }
}

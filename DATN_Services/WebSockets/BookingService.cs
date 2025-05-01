using DATN_Models.DAL.Seat;
using DATN_Models.DAO.Interface.SeatAbout;
using DATN_Services.WebSockets.Interfaces;
using System.Collections.Concurrent;

namespace DATN_Services.WebSockets
{
    public class BookingService : IBookingService
    {
        private readonly ConcurrentDictionary<Guid, List<GetSeatByShowTimeDAL>> _seatCache = new();
        private readonly ISeatDAO _seatDAO;

        public BookingService(ISeatDAO seatDAO)
        {
            _seatDAO = seatDAO;
        }

        public async Task<List<GetSeatByShowTimeDAL>> GetSeatByShowTime(Guid showtimeId)
        {
            if (_seatCache.TryGetValue(showtimeId, out var cachedSeats))
            {
                Console.WriteLine($"[Cache] Returning cached data for ShowtimeId: {showtimeId}");
                return cachedSeats;
            }

            var seats = _seatDAO.GetSeatByShowTime(showtimeId, out int totalRecord, out int responseCode);

            if (responseCode == 200 && seats != null)
            {
                _seatCache[showtimeId] = seats;
            }

            return seats;
        }

   
        public void InvalidateCache(Guid showtimeId)
        {
            if (_seatCache.TryRemove(showtimeId, out _))
            {
                Console.WriteLine($"[Cache] Cache invalidated for ShowtimeId: {showtimeId}");
            }
        }


        public void ClearCache()
        {
            _seatCache.Clear();
            Console.WriteLine("[Cache] All cache cleared.");
        }
    }
}

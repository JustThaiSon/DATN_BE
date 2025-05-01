namespace DATN_Models.DAL.Orders
{
    public class OrderMailResultDAL
    {
        public string MovieName { get; set; }
        public string OrderCode { get; set; }
        public string CinemaName { get; set; }
        public string Address { get; set; }
        public string ShowTime { get; set; }
        public string ShowDate { get; set; }
        public string RoomName { get; set; }
        public string Duration { get; set; }
        public string SeatList { get; set; }
        public string ServiceDetailString { get; set; }
        public string GenreString { get; set; }
        public List<ServiceDetails> ServiceDetails { get; set; }
        public long ConcessionAmount { get; set; }
        public long TotalPrice { get; set; }
        public long DiscountPrice { get; set; }
        public long TotalPriceTicket { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? MinimumAge { get; set; }
        public long PointChange { get; set; }
        public string TransactionCode { get; set; }
        public string Thumbnail { get; set; }
        public string PaymentMethodName { get; set; }
        public DateTime PaymentDate { get; set; }
    }
    public class ServiceDetails
    {
        public string ServiceName { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
    }

    public static class ServiceDetailsParser
    {
        public static List<ServiceDetails> ParseServiceDetails(string serviceDetails)
        {
            if (string.IsNullOrWhiteSpace(serviceDetails))
                return new List<ServiceDetails>();

            return serviceDetails
                .Split('|', StringSplitOptions.RemoveEmptyEntries)
                .Select(detail =>
                {
                    try
                    {
                        // Cắt trước dấu "x" để lấy name và quantity
                        var xIndex = detail.IndexOf("x", StringComparison.OrdinalIgnoreCase);
                        if (xIndex == -1) return null;

                        var name = detail.Substring(0, xIndex).Trim();
                        var remaining = detail.Substring(xIndex + 1).Trim();

                        // Tách quantity và price
                        var commaIndex = remaining.IndexOf(",", StringComparison.OrdinalIgnoreCase);
                        if (commaIndex == -1) return null;

                        var quantityString = remaining.Substring(0, commaIndex).Trim();
                        var priceString = remaining.Substring(commaIndex).Trim(' ', ',', '(', ')', 'd');

                        if (!int.TryParse(quantityString, out var quantity))
                            quantity = 0;

                        if (!long.TryParse(priceString.Replace(",", "").Trim(), out var price))
                            price = 0;

                        return new ServiceDetails
                        {
                            ServiceName = name,
                            Quantity = quantity,
                            Price = price
                        };
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(service => service != null)
                .ToList();
        }
    }

}

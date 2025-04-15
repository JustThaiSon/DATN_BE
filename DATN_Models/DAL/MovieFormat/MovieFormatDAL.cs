﻿namespace DATN_Models.DAL.MovieFormat
{
    public class MovieFormatDAL
    {
        public Guid FormatId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PriceMultiplier { get; set; }
        public int Status { get; set; }
    }
}

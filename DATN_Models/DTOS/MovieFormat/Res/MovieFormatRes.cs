﻿namespace DATN_Models.DTOS.MovieFormat.Res
{
    public class MovieFormatRes
    {
        public Guid FormatId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PriceMultiplier { get; set; }
        public int Status { get; set; }
    }
}

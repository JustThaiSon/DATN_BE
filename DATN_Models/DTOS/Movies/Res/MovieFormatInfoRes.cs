﻿namespace DATN_Models.DTOS.Movies.Res
{
    public class MovieFormatInfoRes
    {
        public Guid FormatId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PriceMultiplier { get; set; }
    }
}

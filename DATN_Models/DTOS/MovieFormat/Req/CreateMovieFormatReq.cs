﻿namespace DATN_Models.DTOS.MovieFormat.Req
{
    public class CreateMovieFormatReq
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PriceMultiplier { get; set; }
    }
}

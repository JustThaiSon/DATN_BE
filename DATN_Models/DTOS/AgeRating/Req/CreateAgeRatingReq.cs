﻿namespace DATN_Models.DTOS.AgeRating.Req
{
    public class CreateAgeRatingReq
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public int MinimumAge { get; set; }
    }
}

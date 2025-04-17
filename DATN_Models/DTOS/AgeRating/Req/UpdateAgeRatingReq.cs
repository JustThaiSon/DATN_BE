﻿namespace DATN_Models.DTOS.AgeRating.Req
{
    public class UpdateAgeRatingReq
    {
        public Guid AgeRatingId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int MinimumAge { get; set; }
    }
}

﻿namespace DATN_Models.DTOS.AgeRating.Res
{
    public class AgeRatingRes
    {
        public Guid AgeRatingId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int MinimumAge { get; set; }
        public int Status { get; set; }
    }
}

﻿namespace DATN_Models.DTOS.MovieFormat.Req
{
    public class AssignFormatToMovieReq
    {
        public Guid MovieId { get; set; }
        public Guid FormatId { get; set; }
    }
}

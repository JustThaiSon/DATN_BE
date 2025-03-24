using System;

namespace DATN_Models.DTOS.Movies.Res
{
    public class MovieGenreRes
    {
        public Guid Id { get; set; }
        public string GenreName { get; set; }
        public int Status { get; set; }
    }
}
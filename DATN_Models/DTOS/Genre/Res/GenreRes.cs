using System;

namespace DATN_Models.DTOS.Genre.Res
{
    public class GetGenreRes
    {
        public Guid Id { get; set; }
        public string GenreName { get; set; }
        public int Status { get; set; }
    }
}
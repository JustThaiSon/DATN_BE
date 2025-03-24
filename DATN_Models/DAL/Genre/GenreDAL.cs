using System;

namespace DATN_Models.DAL.Genre
{
    public class GenreDAL
    {
        public Guid Id { get; set; }
        public string GenreName { get; set; }
        public int Status { get; set; }
    }

    public class AddGenreDAL
    {
        public string GenreName { get; set; }
        public int Status { get; set; } = 1;
    }

    public class UpdateGenreDAL
    {
        public string GenreName { get; set; }
        public int Status { get; set; }
    }
}
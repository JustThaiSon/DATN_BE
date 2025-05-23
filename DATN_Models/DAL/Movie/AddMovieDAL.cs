﻿namespace DATN_Models.DAL.Movie
{
    public class AddMovieDAL
    {
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string BannerURL { get; set; } = "https://cached.imagescaler.hbpl.co.uk/resize/scaleHeight/815/cached.offlinehbpl.hbpl.co.uk/news/OMC/Posters.jpg";
        public string ThumbnailURL { get; set; } = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ7VWH_ycrS7QxKG0CGIt1nBMmj8fYSs1Xe5w&s";
        public string TrailerURL { get; set; } = "https://www.youtube.com/embed/K4TOrB7at0Y";
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime ImportDate { get; set; } // ngày nhập phim vào hệ thống
        public DateTime EndDate { get; set; } // ngày hết hạn của phim
        public int Status { get; set; }
        public List<Guid>? ListActorID { get; set; }
        public List<Guid>? ListGenreID { get; set; }
        public Guid? AgeRatingId { get; set; }
        public List<Guid>? ListFormatID { get; set; }
    }
}

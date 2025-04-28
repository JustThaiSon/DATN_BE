using DATN_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Models.DTOS.Counter.Res
{
    public class Counter_NowPlayingMovies_GetList_DTO
    {
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Banner { get; set; }
        public string Trailer { get; set; }
        public int Duration { get; set; }
        public int Status { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Genres { get; set; } // Giữ nguyên dạng chuỗi JSON
        public string Showtimes { get; set; } // Giữ nguyên dạng chuỗi JSON
    }
}


using DATN_Models.Models;
using Newtonsoft.Json;

namespace DATN_Models.DAL.Counter
{
    public class Counter_NowPlayingMovies_GetList_DAL
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
        public string Genres { get; set; }
        public string Showtimes { get; set; }

        // Thuộc tính deserialized
        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<GenreDTO> GenresList
        {
            get
            {
                if (!string.IsNullOrEmpty(Genres))
                {
                    return JsonConvert.DeserializeObject<List<GenreDTO>>(Genres);
                }
                return new List<GenreDTO>();
            }
        }

        [JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public List<ShowtimeDTO> ShowtimesList
        {
            get
            {
                if (!string.IsNullOrEmpty(Showtimes))
                {
                    return JsonConvert.DeserializeObject<List<ShowtimeDTO>>(Showtimes);
                }
                return new List<ShowtimeDTO>();
            }
        }
    }

    public class GenreDTO
    {
        [JsonProperty("GenreId")]
        public Guid GenreId { get; set; }

        [JsonProperty("GenreName")]
        public string GenreName { get; set; }
    }

    public class ShowtimeDTO
    {
        [JsonProperty("ShowtimeId")]
        public Guid ShowtimeId { get; set; }

        [JsonProperty("RoomId")]
        public Guid RoomId { get; set; }

        [JsonProperty("RoomName")]
        public string RoomName { get; set; }

        [JsonProperty("CinemaId")]
        public Guid CinemaId { get; set; }

        [JsonProperty("StartTime")]
        public DateTime StartTime { get; set; }

        [JsonProperty("EndTime")]
        public DateTime EndTime { get; set; }

        [JsonProperty("Status")]
        public int Status { get; set; }

        [JsonProperty("SeatPrice")]
        public decimal SeatPrice { get; set; }
    }
}


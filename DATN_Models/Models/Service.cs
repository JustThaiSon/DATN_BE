﻿namespace DATN_Models.Models
{
    public class Service
    {
        public Guid Id { get; set; }
        public Guid ServiceTypeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Status { get; set; }
    }
}

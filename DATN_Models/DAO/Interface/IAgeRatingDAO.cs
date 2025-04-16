﻿using DATN_Models.DAL.AgeRating;

namespace DATN_Models.DAO.Interface
{
    public interface IAgeRatingDAO
    {
        List<AgeRatingDAL> GetAgeRatings(int currentPage, int recordPerPage, out int totalRecord, out int response);
        AgeRatingDAL GetAgeRatingById(Guid id, out int response);
        void CreateAgeRating(AgeRatingDAL ageRating, out int response);
        void UpdateAgeRating(AgeRatingDAL ageRating, out int response);
        void DeleteAgeRating(Guid id, out int response);
    }
}

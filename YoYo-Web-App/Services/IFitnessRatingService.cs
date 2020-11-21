using System.Collections.Generic;
using YoYo_Web_App.Models;

namespace YoYo_Web_App.Services
{
    public partial interface IFitnessRatingService
    {
        List<FitnessRating> GetFitnessRatingsFromJson();

        List<Players> GetPlayersFromJson();

        List<string> GetList(List<FitnessRating> fitnessRatings, string Column);
    }
}

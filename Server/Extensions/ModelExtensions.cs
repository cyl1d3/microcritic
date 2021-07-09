using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using microcritic.Server.Models;

namespace microcritic.Server.Extensions
{
    public static class ModelExtensions
    {
        public static Shared.ViewModels.Game ToViewModel(this Game game)
            => new Shared.ViewModels.Game
            {
                Id = game.Id,
                Name = game.Name,
                Description = game.Description,
                Developer = game.Developer.Name,
                Score = (decimal)game.Reviews.Average(r => (byte)r.Rating),
                ReviewCount = game.Reviews.Count,
            };

        public static Shared.ViewModels.Review ToViewModel(this Review review)
            => new Shared.ViewModels.Review
            {
                Id = review.Id,
                Rating = review.Rating,
                UserName = review.User.UserName,
                Text = review.Text,
                Date = review.Date,
            };
    }
}

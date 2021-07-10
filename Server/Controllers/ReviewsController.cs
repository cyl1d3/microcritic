using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using microcritic.Server.Data;
using microcritic.Server.Extensions;
using microcritic.Shared.ViewModels;

namespace microcritic.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class ReviewsController : Controller
    {
        private const int PAGESIZE = 5;

        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly UserManager<Models.ApplicationUser> _userManager;

        public ReviewsController(ApplicationDbContext context, ILogger<GamesController> logger, UserManager<Models.ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet("{id}/{page:int?}")]
        public async Task<IEnumerable<Review>> Game(string id, int? page)
        {
            if(Guid.TryParse(id, out var guid))
            {
                return await _context.Reviews.OrderByDescending(r => r.Date)
                    .Include(r => r.Game)
                    .Include(r => r.User)
                    .Where(r => r.Game.Id == guid)
                    .Skip(PAGESIZE * page ?? 0).Take(PAGESIZE)
                    .Select(r => r.ToViewModel())
                    .ToListAsync();
            }
            else
            {
                return null;
            }
        }

        [HttpGet("{gameid}/User/{username}")]
        [Authorize]
        public async Task<Review> Game(string gameid, string username)
        {
            if (Guid.TryParse(gameid, out var gameGuid))
            {
                return await _context.Reviews
                    .Include(r => r.Game)
                    .Include(r => r.User)
                    .Where(r => r.Game.Id == gameGuid)
                    .Where(r => r.User.UserName == username)
                    .Select(r => r.ToViewModel())
                    .SingleOrDefaultAsync() 
                    ?? new Review 
                    {
                        Game = gameGuid,
                        UserName = username,
                        Rating = Shared.Enums.Rating.Ausgezeichnet,
                    };
            }
            else
            {
                return null;
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] Review review)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(review.UserName);

                await _context.Reviews.AddAsync(new Models.Review
                    {
                        Id = Guid.NewGuid(),
                        GameId = review.Game,
                        User = user,
                        Date = DateTime.Now,
                        Rating = review.Rating,
                        Text = review.Text,
                    });
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{reviewid}")]
        public async Task<IActionResult> Delete(string reviewId)
        {
            if(Guid.TryParse(reviewId, out var reviewGuid))
            {
                var review = await _context.Reviews.Where(r => r.Id == reviewGuid).SingleOrDefaultAsync();

                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

    }
}

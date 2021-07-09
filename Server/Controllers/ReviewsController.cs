using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
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

        private ApplicationDbContext _context;
        private ILogger _logger;

        public ReviewsController(ApplicationDbContext context, ILogger<GamesController> logger)
        {
            _context = context;
            _logger = logger;
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
    }
}

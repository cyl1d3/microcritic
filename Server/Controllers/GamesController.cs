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
    public class GamesController : Controller //🎮
    {
        private const int PAGESIZE = 5;

        private ApplicationDbContext _context;
        private ILogger _logger;

        public GamesController(ApplicationDbContext context, ILogger<GamesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<Game> Game(string id)
        {
            if(Guid.TryParse(id, out var guid))
            {
                return (await _context.Games.Where(g => g.Id == guid)
                        .Include(g => g.Developer)
                        .Include(g => g.Reviews)
                        .SingleOrDefaultAsync()
                    ).ToViewModel();
            }
            else
            {
                return null;
            }
        }

        [HttpGet("{page:int?}")]
        public async Task<IEnumerable<Game>> List(int? page)
        {
            return await _context.Games.OrderBy(g => g.Name)
                .Include(g => g.Developer)
                .Include(g => g.Reviews)
                .Skip(PAGESIZE * page ?? 0).Take(PAGESIZE)
                .Select(g => g.ToViewModel())
                .ToListAsync();
        }
    }
}

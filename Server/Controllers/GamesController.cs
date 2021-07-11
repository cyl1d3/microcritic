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
            if (Guid.TryParse(id, out var guid))
            {
                return (await _context.Games.Where(g => g.Id == guid)
                        .Include(g => g.Developer)
                        .Include(g => g.Reviews)
                        .AsNoTracking()
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
                .AsNoTracking()
                .Select(g => g.ToViewModel())
                .ToListAsync();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] Game game)
        {
            if (ModelState.IsValid)
            {
                Models.Developer developer;
                if (game.Developer.Id == Guid.Empty)
                {
                    developer = NewDeveloper(game.Developer.Name);
                }
                else
                {
                    developer = await _context.Developers.Where(d => d.Id == game.Developer.Id).SingleOrDefaultAsync();
                }

                bool gameExists = await _context.Games.AnyAsync(g => g.Name == game.Name && game.Developer.Id == developer.Id);
                if (gameExists)
                {
                    return Conflict("Game exists");
                }

                await _context.Games.AddAsync(new Models.Game
                {
                    Id = Guid.NewGuid(),
                    Name = game.Name,
                    Description = game.Description,
                    Developer = developer
                });

                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCsv([FromBody] IEnumerable<CsvGame> games)
        {
            if (ModelState.IsValid)
            {
                var gamesInFile = games.GroupBy(g => g.Name)
                    .ToDictionary(group => group.Key, group => group.First());

                //get games with names equal to name in file from db
                //assumes games are unique by name
                var existingGames = await _context.Games
                    .Where(g => gamesInFile.Keys.Contains(g.Name))
                    .AsNoTracking()
                    .Select(g => g.Name)
                    .ToListAsync();

                if(existingGames.Count() == gamesInFile.Count())
                {
                    //all games exist, return empty
                    return new JsonResult(new object[] { });
                }

                //get developers in file from db
                //assumes devs are unique by name
                var allDevs = games.Select(g => g.Developer).Distinct();
                var knownDevs = await _context.Developers
                    .Where(d => allDevs.Contains(d.Name))
                    .ToDictionaryAsync(d => d.Name, d => d);

                foreach(var dev in allDevs.Except(knownDevs.Keys))
                {
                    //create unknown devs
                    var newDev = NewDeveloper(dev);
                    knownDevs[dev] = newDev;
                }

                var resultset = new List<Models.Game>();
                foreach(var game in gamesInFile.Keys.Except(existingGames))
                {
                    //create games that don't exist
                    var newGame = new Models.Game
                    {
                        Id = Guid.NewGuid(),
                        Name = game,
                        Description = gamesInFile[game].Description,
                        Developer = knownDevs[gamesInFile[game].Developer],
                    };

                    await _context.Games.AddAsync(newGame);
                    resultset.Add(newGame);
                }

                await _context.SaveChangesAsync();

                //Return newly added objects
                return new OkObjectResult(resultset.Select(g => g.ToViewModel()));
            }
            return BadRequest();
        }

        [HttpDelete("{gameid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string gameid)
        {
            if(Guid.TryParse(gameid, out var gameGuid))
            {
                var game = await _context.Games.FindAsync(gameGuid);

                if (game is not null)
                {
                    _context.Games.Remove(game);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private Models.Developer NewDeveloper(string name)
        {
            Models.Developer developer = new Models.Developer
            {
                Id = Guid.NewGuid(),
                Name = name,
            };
            _context.Developers.Add(developer);
            return developer;
        }
    }
}

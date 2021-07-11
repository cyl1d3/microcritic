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
    public class DevelopersController : Controller
    {

        private ApplicationDbContext _context;
        private ILogger _logger;

        public DevelopersController(ApplicationDbContext context, ILogger<GamesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{namesearch?}")]
        //for consumption by typeahead control
        public async Task<IEnumerable<Developer>> List(string nameSearch)
        {
            nameSearch ??= "";

            return await _context.Developers
                .Where(d => EF.Functions.Like(d.Name, $"%{nameSearch}%"))
                .OrderBy(d => d.Name)
                .Take(5)
                .AsNoTracking()
                .Select(d => d.ToViewModel())
                .ToListAsync();
        }
    }
}

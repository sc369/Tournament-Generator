using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TournamentGenerator.Data;
using TournamentGenerator.Models;
using TournamentGenerator.State;

namespace TournamentGenerator.Controllers
{
    public class TournamentsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context;

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public TournamentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Tournaments
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await GetCurrentUserAsync();
                var applicationDbContext = _context.Tournaments.Include(t => t.User).Where(t => t.userId == user.Id);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                //var tournaments = await _context.Tournaments.ToListAsync();
                return View("NotAuthenticated");
            }
        }

        public async Task<IActionResult> SetActiveTournament(int? id)
        {
            var tournament = await _context.Tournaments
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            var tournaments = await _context.Tournaments.ToListAsync();
            TournamentState.currentTournament = tournament;
            return View("Index", tournaments);
        }

        // GET: Tournaments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        //// GET: Tournaments/Create
        //public IActionResult Create()
        //{
        //    ViewData["userId"] = new SelectList(_context.Users, "Id", "Id");
        //    return View();
        //}

        // POST: Tournaments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,userId,Name,Date,Location,NumberOfRounds")] Tournament tournament)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(tournament);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["userId"] = new SelectList(_context.Users, "Id", "Id", tournament.userId);
        //    return View(tournament);
        //}


        // GET: Tournaments/Create
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("NotAuthenticated");
            }
            return View("CreateTournament");
        }

        // POST: Tournaments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Date,Location,NumberOfRounds")] Tournament tournament)
        {
            var user = await GetCurrentUserAsync();

            tournament.userId = user.Id;

            // Create new tournament and round
            Round round = null;

            if (ModelState.IsValid)
            {
                _context.Add(tournament);
                await _context.SaveChangesAsync();

                Round newRound = new Round()
                {
                    Number = 1,
                    TournamentId = tournament.Id
                };

                _context.Add(newRound);
                await _context.SaveChangesAsync();
                round = newRound;
            }

            TournamentState.setcurrentTournament(tournament);
            TournamentState.currentRound.Number = 1;

            //Get players unassigned to games

            var players = await _context.Players.ToListAsync();
            var unassignedPlayers = players.Where(p => p.MyGames == null && p.TheirGames == null).ToList();

            foreach (var item in TournamentState.currentPlayers)
            {
                Console.WriteLine(item.FirstName);
            }

            // If number of players is odd, find or generate a 'bye' player

            if (unassignedPlayers.Count % 2 != 0)
            {
                var byePlayerOrNull = players.SingleOrDefault(p => p.FirstName == "Bye");
                if (byePlayerOrNull == null)
                {
                    var byePlayer = new Player()
                    {
                        FirstName = "Bye",
                        LastName = "",
                    };
                    _context.Add(byePlayer);
                    await _context.SaveChangesAsync();
                    unassignedPlayers.Add(byePlayer);
                }
                else
                {
                    unassignedPlayers.Add(byePlayerOrNull);
                }
            }

            foreach (var player in unassignedPlayers)
            {
                TournamentState.currentPlayers.Add(player);
            }

            //Sort players randomly

            var rand = new Random();
            var randomPlayers = unassignedPlayers.OrderBy(p => rand.NextDouble()).ToList();
            int numberOfGames = randomPlayers.Count / 2;

            var bye = randomPlayers.Find(p => p.FirstName == "Bye");

            //Assign players to games, create tables
            for (int i = 0; i < numberOfGames; i++)
            {
                var newTable = new PhysicalTable();
                _context.Add(newTable);
                await _context.SaveChangesAsync();
                TournamentState.currentTables.Add(newTable);

                var tables = await _context.PhysicalTables.ToListAsync();
                tables.OrderBy(t => t.Id);

                var game = new Game();
                game.PhysicalTableId = tables[i].Id;
                game.RoundId = round.Id;
                game.PlayerOneId = randomPlayers[0].Id;
                randomPlayers.Remove(randomPlayers[0]);
                game.PlayerTwoId = randomPlayers[0].Id;
                randomPlayers.Remove(randomPlayers[0]);

                //The opponent of the bye player automatically wins
                if (game.PlayerOneId == bye.Id)
                {
                    game.PlayerTwoScore = 1;
                }
                else if (game.PlayerTwoId == bye.Id)
                {
                    game.PlayerOneScore = 1;
                }
                _context.Add(game);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("IndexUncompleted", "Games");
        }

        // GET: Tournaments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }
            ViewData["userId"] = new SelectList(_context.Users, "Id", "Id", tournament.userId);
            return View(tournament);
        }

        // POST: Tournaments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,userId,Name,Date,Location,NumberOfRounds")] Tournament tournament)
        {
            if (id != tournament.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tournament);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TournamentExists(tournament.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["userId"] = new SelectList(_context.Users, "Id", "Id", tournament.userId);
            return View(tournament);
        }

        // GET: Tournaments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tournament == null)
            {
                return NotFound();
            }
            if (tournament.Id == TournamentState.currentTournament.Id)
            {
                TournamentState.currentTournament = null;
            }



            return View(tournament);
        }

        // POST: Tournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            _context.Tournaments.Remove(tournament);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TournamentExists(int id)
        {
            return _context.Tournaments.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TournamentGenerator.Data;
using TournamentGenerator.Models;
using TournamentGenerator.Models.ViewModels.PlayerViewModels;

namespace TournamentGenerator.Controllers
{
    public class PlayersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlayersController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: Players
        public async Task<IActionResult> Index()
        {
            if (State.TournamentState.currentTournament == null)
            {
                return View("TournamentError");
            }

            var players = await _context.Players.ToListAsync();
            var rounds = await _context.Rounds.ToListAsync();
            var games = await _context.Games.ToListAsync();
            var currentRounds = rounds.Where(r => r.TournamentId == State.TournamentState.currentTournament.Id).ToList();
            var currentGames = new List<Game>();
            var currentPlayers = new List<Player>();
            foreach (var currentRound in currentRounds)
            {
                foreach (var game in games)
                {
                    if (game.RoundId == currentRound.Id)
                    {
                        currentGames.Add(game);
                    }
                }
            }

            foreach (var game in currentGames)
            {
                foreach (var player in players)
                {
                    if (game.PlayerOneId == player.Id)
                    {
                        if (!currentPlayers.Contains(player))
                        {
                            currentPlayers.Add(player);
                        }
                    }
                    if (game.PlayerTwoId == player.Id)
                    {
                        if (!currentPlayers.Contains(player))
                        {
                            currentPlayers.Add(player);
                        }
                    }
                }
            }
            foreach (var game in currentGames)
            {
                foreach (var player in currentPlayers)
                {
                    if (player.Id == game.PlayerOneId)
                    {
                        player.Score += game.PlayerOneScore;
                    }
                    if (player.Id == game.PlayerTwoId)
                    {
                        player.Score += game.PlayerTwoScore;
                    }
                };
            }

            return View(currentPlayers.OrderByDescending(player => player.Score));
        }
        public async Task<IActionResult> PlayerStandings()
        {
            if (State.TournamentState.currentTournament == null)
            {
                return View("TournamentError");
            }

            var players = await _context.Players.ToListAsync();
            var rounds = await _context.Rounds.ToListAsync();
            var games = await _context.Games.ToListAsync();
            var currentRounds = rounds.Where(r => r.TournamentId == State.TournamentState.currentTournament.Id).ToList();
            var currentGames = new List<Game>();
            var currentPlayers = new List<Player>();
            foreach (var currentRound in currentRounds)
            {
                foreach (var game in games)
                {
                    if (game.RoundId == currentRound.Id)
                    {
                        currentGames.Add(game);
                    }
                }
            }

            foreach (var game in currentGames)
            {
                foreach (var player in players)
                {
                    if (game.PlayerOneId == player.Id)
                    {
                        if (!currentPlayers.Contains(player))
                        {
                            currentPlayers.Add(player);
                        }
                    }
                    if (game.PlayerTwoId == player.Id)
                    {
                        if (!currentPlayers.Contains(player))
                        {
                            currentPlayers.Add(player);
                        }
                    }
                }
            }
            foreach (var game in currentGames)
            {
                foreach (var player in currentPlayers)
                {
                    if (player.Id == game.PlayerOneId)
                    {
                        player.Score += game.PlayerOneScore;
                    }
                    if (player.Id == game.PlayerTwoId)
                    {
                        player.Score += game.PlayerTwoScore;
                    }
                };
            }

            return View(currentPlayers.OrderByDescending(player => player.Score));
        }

        public async Task<IActionResult> IndexUnassigned()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("NotAuthenticated");
            }

            var players = await _context.Players
                .Include(p => p.MyGames)
                .Include(p => p.TheirGames).ToListAsync();

            List<Player> unassignedPlayers = new List<Player>();
            foreach (var player in players)
            {
                if (player.MyGames == null && player.TheirGames == null)
                {
                    unassignedPlayers.Add(player);
                }
                if (player.MyGames.Count == 0 && player.TheirGames.Count == 0)
                {
                    unassignedPlayers.Add(player);
                }
            }

            var model = new UnassignedPlayerViewModel();
            model.unassignedPlayers = unassignedPlayers;
            return View(model);
        }



        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,ELO")] Player player)
        {
            if (ModelState.IsValid)
            {
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,ELO")] Player player)
        {
            if (id != player.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id))
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
            return View(player);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.FindAsync(id);
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}

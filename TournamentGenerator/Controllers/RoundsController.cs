using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TournamentGenerator.Data;
using TournamentGenerator.Models;

namespace TournamentGenerator.Controllers
{
    public class RoundsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoundsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rounds
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Rounds.Include(r => r.Tournament);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Rounds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var round = await _context.Rounds
                .Include(r => r.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (round == null)
            {
                return NotFound();
            }

            return View(round);
        }
        async public Task<IActionResult> GenerateRound()
        {
            var players = await _context.Players.ToListAsync();
            var rounds = await _context.Rounds.ToListAsync();
            var games = await _context.Games.ToListAsync();
            var physicalTables = await _context.PhysicalTables.ToListAsync();
            var currentRounds = rounds.Where(r => r.TournamentId == State.TournamentState.currentTournament.Id).ToList();
            var currentGames = new List<Game>();
            var currentPlayers = new List<Player>();
            var currentTables = new List<PhysicalTable>();

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
                    if (game.PlayerOneId == player.Id && !currentPlayers.Contains(player))
                    {
                        {
                            currentPlayers.Add(player);
                        }
                    }

                    if (game.PlayerTwoId == player.Id && !currentPlayers.Contains(player))

                    {
                        currentPlayers.Add(player);
                    }
                }

                foreach (var table in physicalTables)

                {
                    if (game.PhysicalTableId == table.Id)
                    {
                        currentTables.Add(table);
                    }
                }
            }

            Round round = new Round();
            round.TournamentId = State.TournamentState.currentTournament.Id;
            round.Number = State.TournamentState.currentRound.Number + 1;
            _context.Add(round);
            await _context.SaveChangesAsync();

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

            //See if players have played before

            var previousMatches = new List<Dictionary<int, int>>();

            for (int i = 0; i < currentGames.Count; i++)
            {
                previousMatches.Add(new Dictionary<int, int>());
                previousMatches[i].Add(currentGames[i].PlayerOneId, currentGames[i].PlayerTwoId);
            }

            var currentMatches = new Dictionary<int, int>();

            bool haveBeenMatched(int playerOneId, int playerTwoId)
            {
                //foreach (var item in previousMatches)
                //{
                for (int i = 0; i < previousMatches.Count(); i++)
                {
                    previousMatches[i].TryGetValue(playerOneId, out int value1);
                    previousMatches[i].TryGetValue(playerTwoId, out int value2);
                    if (value1 == playerTwoId || value2 == playerOneId)
                    {
                        return true;
                    }
                }
                //}
                return false;
            }
            var orderedPlayers = currentPlayers.OrderByDescending(p => p.Score).ToList();

            int numberOfGames = orderedPlayers.Count / 2;

            var bye = orderedPlayers.Find(p => p.FirstName == "Bye");

            //Assign players to games, create tables
            for (int i = 0; i < numberOfGames; i++)
            {
                currentTables.OrderBy(t => t.Id);
                var game = new Game();
                game.PhysicalTableId = currentTables[i].Id;
                game.RoundId = round.Id;

                //get the highest scoring player, then remove them from the list

                var playerOneId = orderedPlayers[0].Id;
                game.PlayerOneId = playerOneId;
                orderedPlayers.Remove(orderedPlayers[0]);

                //loop through all the other players and choose the highest scoring one who has not previously played the first chosen player

                for (int j = 0; j < orderedPlayers.Count; j++)
                {
                    var playerTwoId = orderedPlayers[j].Id;

                    if (!haveBeenMatched(playerOneId, playerTwoId))
                    {
                        game.PlayerTwoId = playerTwoId;
                        orderedPlayers.Remove(orderedPlayers[j]);
                        j = orderedPlayers.Count() + 1;
                        if (game.PlayerOneId == bye.Id)
                        {
                            game.PlayerTwoScore = 1;
                        }
                        else if (game.PlayerTwoId == bye.Id)
                        {
                            game.PlayerOneScore = 1;
                        }
                        _context.Add(game);
                    await _context.SaveChangesAsync();
                    }
                }
                //The opponent of the bye player automatically wins
                             
            }
                
            return RedirectToAction("IndexUncompleted", "Games");
        }

        // GET: Rounds/Create
        public IActionResult Create()
        {
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Location");
            return View();
        }

        // POST: Rounds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TournamentId,Number")] Round round)
        {
            if (ModelState.IsValid)
            {
                _context.Add(round);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Location", round.TournamentId);
            return View(round);
        }

        // GET: Rounds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var round = await _context.Rounds.FindAsync(id);
            if (round == null)
            {
                return NotFound();
            }
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Location", round.TournamentId);
            return View(round);
        }

        // POST: Rounds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TournamentId,Number")] Round round)
        {
            if (id != round.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(round);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoundExists(round.Id))
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
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Location", round.TournamentId);
            return View(round);
        }

        // GET: Rounds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var round = await _context.Rounds
                .Include(r => r.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (round == null)
            {
                return NotFound();
            }

            return View(round);
        }

        // POST: Rounds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var round = await _context.Rounds.FindAsync(id);
            _context.Rounds.Remove(round);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoundExists(int id)
        {
            return _context.Rounds.Any(e => e.Id == id);
        }
    }
}

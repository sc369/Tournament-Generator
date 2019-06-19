using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TournamentGenerator.Data;
using TournamentGenerator.Models;
using TournamentGenerator.Models.ViewModels.GameViewModels;

namespace TournamentGenerator.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Games.Include(g => g.PhysicalTable).Include(g => g.PlayerOne).Include(g => g.PlayerTwo).Include(g => g.Round);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> IndexUncompleted()
        {
            var applicationDbContext = _context.Games.Include(g => g.PhysicalTable).Include(g => g.PlayerOne).Include(g => g.PlayerTwo).Include(g => g.Round).Where(g => g.PlayerOneScore == 0 && g.PlayerTwoScore == 0);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.PhysicalTable)
                .Include(g => g.PlayerOne)
                .Include(g => g.PlayerTwo)
                .Include(g => g.Round)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            ViewData["PhysicalTableId"] = new SelectList(_context.PhysicalTables, "Id", "Id");
            ViewData["PlayerOneId"] = new SelectList(_context.Players, "Id", "FirstName");
            ViewData["PlayerTwoId"] = new SelectList(_context.Players, "Id", "FirstName");
            ViewData["RoundId"] = new SelectList(_context.Rounds, "Id", "Id");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoundId,PlayerOneId,PlayerTwoId,PlayerOneScore,PlayerTwoScore,PhysicalTableId")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PhysicalTableId"] = new SelectList(_context.PhysicalTables, "Id", "Id", game.PhysicalTableId);
            ViewData["PlayerOneId"] = new SelectList(_context.Players, "Id", "FirstName", game.PlayerOneId);
            ViewData["PlayerTwoId"] = new SelectList(_context.Players, "Id", "FirstName", game.PlayerTwoId);
            ViewData["RoundId"] = new SelectList(_context.Rounds, "Id", "Id", game.RoundId);
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            GameResultViewModel viewModel = new GameResultViewModel();

            var game = await _context.Games.FindAsync(id);
            viewModel.Game = new Game() { };

            if (game == null)
            {
                return NotFound();
            }
            viewModel.Game = game;

            ViewData["PhysicalTableId"] = new SelectList(_context.PhysicalTables, "Id", "Id", game.PhysicalTableId);
            ViewData["PlayerOneId"] = new SelectList(_context.Players, "Id", "FirstName", game.PlayerOneId);
            ViewData["PlayerTwoId"] = new SelectList(_context.Players, "Id", "FirstName", game.PlayerTwoId);
            ViewData["RoundId"] = new SelectList(_context.Rounds, "Id", "Id", game.RoundId);
            return View(viewModel);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GameResultViewModel viewModel)
        {
            //if (id != game.Id)
            //{
            //    return NotFound();
            //}

            var players = await _context.Players.ToListAsync();

            if (viewModel.Result == 1)
            {
                viewModel.Game.PlayerOneScore = 1;
                viewModel.Game.PlayerTwoScore = 0;

            }
            else if (viewModel.Result == 2)
            {
                viewModel.Game.PlayerOneScore = 0;
                viewModel.Game.PlayerTwoScore = 1;
            }
            else if (viewModel.Result == 3)
            {
                viewModel.Game.PlayerOneScore = 0.5;
                viewModel.Game.PlayerTwoScore = 0.5;               
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewModel.Game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(viewModel.Game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("IndexUncompleted");
                //return RedirectToAction(nameof(Index));
            }
            ViewData["PhysicalTableId"] = new SelectList(_context.PhysicalTables, "Id", "Id", viewModel.Game.PhysicalTableId);
            ViewData["PlayerOneId"] = new SelectList(_context.Players, "Id", "FirstName", viewModel.Game.PlayerOneId);
            ViewData["PlayerTwoId"] = new SelectList(_context.Players, "Id", "FirstName", viewModel.Game.PlayerTwoId);
            ViewData["RoundId"] = new SelectList(_context.Rounds, "Id", "Id", viewModel.Game.RoundId);
            return RedirectToAction("IndexUncompleted");
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.PhysicalTable)
                .Include(g => g.PlayerOne)
                .Include(g => g.PlayerTwo)
                .Include(g => g.Round)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}

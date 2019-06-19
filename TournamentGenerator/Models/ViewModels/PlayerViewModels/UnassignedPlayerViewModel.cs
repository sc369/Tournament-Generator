
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TournamentGenerator.Models;


namespace TournamentGenerator.Models.ViewModels.PlayerViewModels
{
    public class UnassignedPlayerViewModel
    {
        public IEnumerable<Player> unassignedPlayers { get; set; }

        public string NoPlayersError { get; set; }
    }
}
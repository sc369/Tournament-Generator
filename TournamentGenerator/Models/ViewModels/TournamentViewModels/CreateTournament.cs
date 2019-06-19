
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TournamentGenerator.Models.ViewModels.TournamentViewModels
{
    public class CreateTournament
    {
        [Required]
        public string Name { get; set; }

        [Required]

        public DateTime Date { get; set; }

        [Required]

        public string Location { get; set; }

        [Display(Name = "Number of Rounds")]
        [Required]
        public int NumberOfRounds { get; set; }            

        [Required]
        public int NumberOfPlayers { get; set; }
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public int ELO { get; set; }
        
    }
}
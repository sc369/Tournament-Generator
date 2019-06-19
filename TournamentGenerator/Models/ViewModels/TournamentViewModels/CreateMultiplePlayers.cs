
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TournamentGenerator.Models.ViewModels.TournamentViewModels
{
    public class CreateMultiplePlayers

    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public int ELO { get; set; }
    }
}

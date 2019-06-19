using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TournamentGenerator.Models
{
    public class Tournament
    {
        [Key]
        public int Id { get; set; }

        public string userId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]

        public string Name { get; set; }

        [Required]

        public DateTime Date { get; set; }

        [Required]

        public string Location { get; set; }

        [Display(Name = " ")]
        [Required]
        public int NumberOfRounds { get; set; }

        public List<Round> Rounds { get; set; }
    }
}
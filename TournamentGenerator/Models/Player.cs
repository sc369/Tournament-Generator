
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TournamentGenerator.Models
{
    public class Player

    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public int ELO { get; set; }

        [NotMapped]
        public double score { get; set; }

        public ICollection<Game> MyGames { get; set; }
        public ICollection<Game> TheirGames { get; set; }
        [NotMapped]
        [Display(Name = "")]
        public double RandomNumber { get; set; }
    }
}
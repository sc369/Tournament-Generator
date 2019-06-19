using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TournamentGenerator.Models
{
    public class Round
    {
        [Key]
        public int Id { get; set; }
        [Required]

        public int TournamentId { get; set; }

        [Required]

        public Tournament Tournament { get; set; }

        [Required]
        public int Number { get; set; }

        public ICollection<Game> Games { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TournamentGenerator.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RoundId { get; set; }

        public Round Round { get; set; }

        public int PlayerOneId { get; set; }

        [Display(Name = "Player One")]
        public Player PlayerOne { get; set; }

        public int PlayerTwoId { get; set; }

        [Display(Name = "Player Two")]
        public Player PlayerTwo { get; set; }

        [Display(Name = "Player One Score")]
        public double PlayerOneScore { get; set; }
        [Display(Name = "Player Two Score")]

        public double PlayerTwoScore { get; set; }

        public int PhysicalTableId { get; set; }

        public PhysicalTable PhysicalTable { get; set; }


    }
}
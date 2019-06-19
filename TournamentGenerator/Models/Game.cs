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

        public Player PlayerOne { get; set; }

        public int PlayerTwoId { get; set; }

        public Player PlayerTwo { get; set; }

        public double PlayerOneScore { get; set; }

        public double PlayerTwoScore { get; set; }

        public int PhysicalTableId { get; set; }

        public PhysicalTable PhysicalTable { get; set; }


    }
}
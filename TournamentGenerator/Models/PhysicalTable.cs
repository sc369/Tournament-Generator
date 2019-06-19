using System;
using System.Collections.Generic;

namespace TournamentGenerator.Models
{
    public class PhysicalTable
    {
        public int Id { get; set; }

        public ICollection<Game> Games { get; set; }

    }
}
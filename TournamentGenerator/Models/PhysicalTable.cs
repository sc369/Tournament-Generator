using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TournamentGenerator.Models
{
        public class PhysicalTable
    {
        public int Id { get; set; }
                
        public int Number { get; set; }

        public ICollection<Game> Games { get; set; }

    }
}
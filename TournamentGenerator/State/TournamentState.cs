using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentGenerator.Models;

namespace TournamentGenerator.State
{
    public class TournamentState
    {
        public static Tournament currentTournament;

            public static Round currentRound = new Round();
            public static void setcurrentTournament(Tournament tournament)
            {
                currentTournament = tournament;
            }

            public static List<PhysicalTable> currentTables = new List<PhysicalTable>();

            public static List<Player> currentPlayers = new List<Player>();

        // public static List<Player> getCurrentPlayers()
        //{

        //}



        }
}

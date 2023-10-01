using System;
using System.Collections.Generic;

namespace backend
{
    public class Game
    {
        public Guid gameid { get; set; } // Unique identifier for the game.

        public Guid id { get; set; } // Unique identifier for the game.

        public List<Player> Players { get; set; } // List of players in the game.

        public Table Table { get; set; } // Details about the poker table.
 

        public decimal PotAmount { get; set; } // Total amount in the pot.

        public Card[] HoleCards { get; set; } // The Hole card of the Player.

        public string Status { get; set; }  // the status of the Game


        // Constructor
        public Game()
        {
            Players = new List<Player>();
        }
    }

    public class Player
    {
        public Guid PlayerId { get; set; } // Unique identifier for the player.

        public string Name { get; set; } // Player's name or alias.


        public decimal ChipCount { get; set; } // Current number of chips the player has.

      
    }

    public class Table
    {
        public List<Card> CommunityCards { get; set; } // The shared cards on the table.
    }



    public class Card
    {
        public string Rank { get; set; } // e.g., "Ace", "10", "King".

        public string Suit { get; set; } // e.g., "Hearts", "Clubs".
    }
}

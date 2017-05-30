using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    /// <summary>
    /// Represents the PlayerListRow
    /// </summary>
    public class PlayerListRow
    {
        /// <summary>
        /// The player's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The player's score
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// The PlayerListRow's constructor
        /// </summary>
        /// <param name="name">The Player's name</param>
        /// <param name="score">The Player's scores</param>
        public PlayerListRow(string name, int score)
        {
            this.Name = name;
            this.Score = score;
        }
    }
}

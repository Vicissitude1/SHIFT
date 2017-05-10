using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    class PlayerListRow
    {
        public string Name { get; set; }
        public int Score { get; private set; }

        public PlayerListRow(string name, int score)
        {
            this.Name = name;
            this.Score = score;
        }
    }
}

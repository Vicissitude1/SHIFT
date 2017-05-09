using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingGame
{
    class PlayerListRow
    {
        public int Place { get; private set; }
        public string Name { get; private set; }
        public int Score { get; private set; }

        public PlayerListRow(int place, string name, int score)
        {
            this.Place = place;
            this.Name = name;
            this.Score = score;
        }
    }
}

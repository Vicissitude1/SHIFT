using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootingGame.Interfaces;
namespace ShootingGame
{
    public class TestDice : IDice
    {
        private int value;
        public TestDice(int value)
        {
            this.value = value;
        }
        public int Roll()
        {
            return value;
        }
    }
}

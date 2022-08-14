using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsTheGame
{
    public class Ship
    {
        public string Name { get; set; }
        public int Length { get; set; }

        public Ship(string name, int length)
        {
            Name = name;
            Length = length;
        }
    }
}

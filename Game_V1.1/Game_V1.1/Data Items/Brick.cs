using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_V1._2.Data_Items
{
    class Brick : Square
    {
        public int strength;

        public Brick(int x, int y,int strength) : base(x, y, 1)
        {
            this.strength = strength;
        }

        public int getStrength()
        {
            return this.strength;
        }
    }
}

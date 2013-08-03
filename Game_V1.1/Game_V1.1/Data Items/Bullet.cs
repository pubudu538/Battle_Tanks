using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game_V1._2.Data_Items
{
    class Bullet
    {
        
        public Vector2 vector;
        public int direction;
        public int playerNo;

        public Bullet(Vector2 vec, int dir, int player)
        {
            this.vector = vec;
            this.direction= dir;
            this.playerNo = player;
        }
    }
}

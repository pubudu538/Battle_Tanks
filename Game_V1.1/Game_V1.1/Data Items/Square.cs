using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game_V1._2.Data_Items
{
    class Square
    {
        Vector2 coord;
        int sqType;     //0=empty   1=brick     2=stone     3=water
        bool hasAPlayer;
        Player currentPlayer;

        public Square(int x, int y, int type)
        {
            this.coord = new Vector2(x, y);
            this.sqType = type;
            this.hasAPlayer = false;
            this.currentPlayer = null;
        }

        public int getSType()
        {
            return this.sqType;
        }
        public Vector2 getCoord()
        {
            return this.coord;
        }

        public void addPlayer(Player pl)
        {
            this.currentPlayer = pl;
            this.hasAPlayer = true;
        }
        
        public void removePlayer()
        {
            this.hasAPlayer = false;
            this.currentPlayer = null;
        }

        public bool hasPlayer()
        {
            return this.hasAPlayer;
        }

        public Player getPlayer()
        {
            return this.currentPlayer;
        }
    }
}

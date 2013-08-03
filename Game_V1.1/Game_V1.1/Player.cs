using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game_V1._2
{
    class Player
    {
        public Vector2 position;
        public int playerNumber;
        public int direction;
        public int health;
        public int points;
        public int coins;
        public bool isAlive;
        public Color color;
        public bool shot;
        public Data_Items.Square currentSquare;

        public Player(int n, int x, int y, int dir, Color clr, Data_Items.Square current)
        {
            this.playerNumber = n;
            this.position = new Vector2(x, y);
            this.direction = dir;
            this.color = clr;
            this.points = 0;
            this.coins = 0;
            this.isAlive = true;
            this.health = 100;
            this.shot = false;
            this.currentSquare = current;
        }

        public void updatePlayer(int x, int y, int dir, bool shot, int health, int coins, int points, Data_Items.Square newSqr)
        {
            this.position = new Vector2(x, y);
            this.direction = dir;
            this.points = points;
            this.coins = coins;           
            this.health = health;
            this.shot = shot;

            if(health <= 0)
            {
                isAlive = false;
                currentSquare.removePlayer();
            }

            else if(this.currentSquare != newSqr)
            {
                if (this.currentSquare.getPlayer() == this)
                {
                    this.currentSquare.removePlayer();
                }

                this.currentSquare = newSqr;
                this.currentSquare.addPlayer(this);
            }
        }
    }
}

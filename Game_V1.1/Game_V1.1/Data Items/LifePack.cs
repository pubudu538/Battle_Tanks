using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game_V1._2.Data_Items
{
    class LifePack
    {
        public int endTime;
        public Vector2 position;
        public int remainTime;

        public LifePack(int x, int y, int end, int remain)
        {
            this.position = new Vector2(x, y);            
            this.endTime = end;
            this.remainTime = remain;
        }

        public void UpdateTime(int time)
        {
            this.remainTime = this.endTime - time;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game_V1._2.Data_Items;

namespace Game_V1._2
{
    class StarNode
    {
        public Square mySqr;
        public StarNode parentSqr;
        public int cost;
        public int direction;

        public StarNode(Square sqr, StarNode parent, int cost, int direction)
        {
            this.mySqr = sqr;
            this.parentSqr = parent;
            this.cost = cost;
            this.direction = direction;
        }
    }
}

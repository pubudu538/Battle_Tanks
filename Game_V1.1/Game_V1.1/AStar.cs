using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game_V1._2.Data_Items;

namespace Game_V1._2
{
    class AStar
    {
        StarNode current = new StarNode(null, null, 0, 0);
        int mapSize;
        Square[] squares;
        
        public AStar(Square[] sqrs, int mapSize)
        {
            this.squares = sqrs;
            this.mapSize = mapSize;
        }

        //calculate the least cost path between two given squares
        public List<StarNode> calculatePath(Square source, Square destination, int direction)
        {
            
            List<StarNode> openList = new List<StarNode>();
            List<StarNode> closedList = new List<StarNode>();
            List<Square> closedSqrs = new List<Square>();
            
            openList.Add(new StarNode(source, null, 0, direction));

            bool finished = false;
            int k = 0;
            
            while(!finished)
            {
                k++;                
                int fullCost = int.MaxValue;

                //get the node with the least cost
                foreach(StarNode node in openList)
                {
                    if (current.mySqr == null)
                    {
                        current = node;
                        fullCost = current.cost + getHeuristic(current.mySqr, destination);
                    }
                    else if ((getHeuristic(node.mySqr, destination) + node.cost) < fullCost)
                    {
                        current = node;
                        fullCost = current.cost + getHeuristic(current.mySqr, destination);
                    }
                }

                int currentDirection = current.direction;

                int xCord = (int)current.mySqr.getCoord().X;
                int yCord = (int)current.mySqr.getCoord().Y;

                int cost = 0;

                int iMin = (xCord > 0) ? -1 : 0;
                int iMax = (xCord < (mapSize - 1)) ? 1 : 0;
                int jMin = (yCord > 0) ? -1 : 0;
                int jMax = (yCord < (mapSize - 1)) ? 1 : 0;

                for (int i = iMin; i <= iMax; i++)
                {
                    for (int j = jMin; j <= jMax; j++)
                    {
                        if ((i != 0) && (j != 0))   //if a diagonal square
                        {
                            continue;
                        }
                        if ((i == 0) && (j == 0))   //if the center square
                        {
                            continue;
                        }

                        bool isDir = false;

                        if ((j == -1) && (currentDirection == 0))
                        {
                            isDir = true;    
                        }
                        else if ((j == 1) && (currentDirection == 2))
                        {
                            isDir = true;
                        }
                        else if ((i == -1) && (currentDirection == 3))
                        {
                            isDir = true;
                        }
                        else if ((i == 1) && (currentDirection == 1))
                        {
                            isDir = true;
                        }

                        int index = (xCord+i) + ((yCord+j) * mapSize);
                        Square currentSqr = squares[index];

                        cost = this.getCost(xCord + i, yCord + j, isDir);

                        int tempDirection = currentDirection;
                        if (j == -1)
                        {
                            tempDirection = 0;
                        }
                        else if (j == 1)
                        {
                            tempDirection = 2;
                        }
                        else if (i == -1)
                        {
                            tempDirection = 4;
                        }
                        else if (i == 1)
                        {
                            tempDirection = 1;
                        }

                        if ((cost >= 0) && (!closedSqrs.Contains(currentSqr)))
                        {
                            openList.Add(new StarNode(currentSqr, current, (cost + current.cost), tempDirection));
                        }
                    }
                }

                openList.Remove(current);
                closedList.Add(current);
                closedSqrs.Add(current.mySqr);

                if((current.mySqr == destination)||(openList.Count == 0))
                {
                    finished = true;
                }

                current = new StarNode(null, null, 0, 0);

            }

            return this.filterClosedL(closedList);
            //return closedList;
        }

        //calculate cost to move to a given square
        public int getCost(int i, int j, Boolean inDirection)
        {
            int index = i + (j * mapSize);
            Square currentSqr = squares[index];

            int cost = 0;

            if ((currentSqr.getSType() == 2) || (currentSqr.getSType() == 3))
            {
                cost = -5;
            }
            else if (currentSqr.getSType() == 0)
            {
                cost = 1;
            }
            else if (currentSqr.getSType() == 1)
            {
                Brick currentBrick = (Brick)currentSqr;
                cost = currentBrick.getStrength() + 1;                
            }

            if (!inDirection)
            {
                cost = cost + 1;
            }

            return cost;
        }

        //calculate a heuristic value for a given source and destination
        public int getHeuristic(Square source, Square target)
        {
            int hDistance = Math.Abs((int)source.getCoord().X - (int)target.getCoord().X);
            int vDistance = Math.Abs((int)source.getCoord().Y - (int)target.getCoord().Y);

            int heurCost = hDistance + vDistance;

            return heurCost;
        }

        public List<StarNode> filterClosedL(List<StarNode> close)
        {
            List<StarNode> result = new List<StarNode>();
            StarNode current = close[close.Count - 1];

            while (current.parentSqr != null)
            {
                result.Add(current);
                current = current.parentSqr;
            }

            result.Add(current);

            result.Reverse();
            return result;
        }
    }
}

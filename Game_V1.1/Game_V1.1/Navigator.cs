using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game_V1._2.Data_Items;

namespace Game_V1._2
{
    class Navigator
    {
        int mapSize;

        public Navigator(int mapSize)
        {
            this.mapSize = mapSize;
        }

        //get the coin pile with the least cost path
        public void GetBestCoin(Square[] sqrs, Coin[] coins, Square current, int direction, out List<StarNode> bestP, out Coin targetCoin)
        {
            AStar ast = new AStar(sqrs, mapSize);
            List<StarNode> bestPath = null;
            Coin target = null;
            int minCost = int.MaxValue;

            foreach(Coin coin in coins)
            {                
                int index = (int)(coin.position.X+(coin.position.Y*mapSize));
                if (sqrs[index].getSType() == 3)
                {
                    continue;
                }
                List<StarNode> path = ast.calculatePath(current, sqrs[index], direction);
                int cost = path[path.Count - 1].cost;
                
                if ((cost < minCost) && ((cost * Util.Constants.Timeout) < coin.remainTime))
                {
                    bestPath = path;
                    minCost = cost;
                    target = coin;
                }
            }

            bestP = bestPath;
            targetCoin = target;
            //return this.generateCommandList(bestPath, direction);
        }

        //get the lifepack with the least cost path
        public void GetBestHealth(Square[] sqrs, LifePack[] packs, Square current, int direction, out List<StarNode> bestP, out LifePack targetPack)
        {
            AStar ast = new AStar(sqrs, mapSize);
            List<StarNode> bestPath = null;
            LifePack target  = null;
            int minCost = int.MaxValue;

            foreach (LifePack pack in packs)
            {
                int index = (int)(pack.position.X + (pack.position.Y * mapSize));
                List<StarNode> path = ast.calculatePath(current, sqrs[index], direction);
                int cost = path[path.Count - 1].cost;

                if ((cost < minCost) && ((cost * Util.Constants.Timeout) < pack.remainTime))
                {
                    bestPath = path;
                    minCost = cost;
                    target = pack;
                }
            }

            bestP = bestPath;
            targetPack = target;
        }

        //generate command list for a given path
        public List<int> generateCommandList(List<StarNode> path, int direction)
        {
            int currentDirection = direction;
            List<int> commands = new List<int>();
            
            if(path == null)
            {
                return commands;
            }

            
            StarNode currentNode = path[0];

            for (int i = 1; i < path.Count; i++)
            {
                int tempDir = 0;
                if (currentNode.mySqr.getCoord().Y > path[i].mySqr.getCoord().Y)
                {
                    tempDir = 0;
                }
                else if (currentNode.mySqr.getCoord().Y < path[i].mySqr.getCoord().Y)
                {
                    tempDir = 2;
                }
                else if (currentNode.mySqr.getCoord().X > path[i].mySqr.getCoord().X)
                {
                    tempDir = 3;
                }
                else if (currentNode.mySqr.getCoord().X < path[i].mySqr.getCoord().X)
                {
                    tempDir = 1;
                }

                if (currentDirection != tempDir)
                {
                    commands.Add(tempDir);
                    currentDirection = tempDir;
                }

                if (path[i].mySqr.getSType() == 1)
                {
                    Brick tempBrick = (Brick)path[i].mySqr;
                    for (int k = 1; k <= tempBrick.getStrength(); k++)
                    {
                        commands.Add(5);
                    }
                }

                commands.Add(currentDirection);
                currentNode = path[i];
            }
            
            return commands;
        }
    }        
}

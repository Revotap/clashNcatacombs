using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Class
{
    internal class Room
    {
        private bool hasLeftNeightbor = false;
        private bool hasTopNeighbor = false;
        private bool hasRightNeightbor = false;
        private bool hasBottomNeighbor = false;

        private Room leftNeightbor;
        private Room topNeighbor;
        private Room rightNeightbor;
        private Room bottomNeighbor;

        private String[,] map;
        int width;
        int height;

        public Room(int max_width, int max_height) {
            this.width = max_width;
            this.height = max_height;
            map = new String[max_width, max_height];

            int rand_num = new Random().Next(1, 15);
            BitArray ba = new BitArray(new int[] { rand_num });
            if (ba.Get(0)) { this.hasLeftNeightbor = true; }
            if(ba.Get(1)) { this.hasTopNeighbor= true; }
            if(ba.Get(2)) { this.hasRightNeightbor= true; }
            if(ba.Get(3)) { this.hasBottomNeighbor= true; }

            generateMap();
        }

        public string[,] Map { get => map; }

        private void generateMap()
        {
            for(int x = 0; x < map.GetLength(0); x++)
            {
                for(int y = 0; y < map.GetLength(1); y++)
                {
                    if(x == 0)
                    {
                        if (y == 0)
                        {
                            //Upper left corner
                            map[x, y] = "wl";
                        }
                        else if (y > 0 && y < height - 1)
                        {
                            //Left wall
                            map[x, y] = "wt";
                        }
                        else
                        {
                            //Bottom left corner
                            map[x, y] = "wr";
                        }
                    }else if(x == width - 1)
                    {
                        if (y == 0)
                        {
                            //Upper left corner
                            map[x, y] = "wr";
                        }
                        else if (y > 0 && y < height - 1)
                        {
                            //Left wall
                            map[x, y] = "wr";
                        }
                        else if (y == height - 1)
                        {
                            //Bottom left corner
                            map[x, y] = "cr";
                        }
                    }
                    else
                    {
                        if(y == 0)
                        {
                            //Top Wall
                            map[x, y] = "wt";
                        }else if(y > 0 && y < height-1)
                        {
                            //Empty space in the middle
                            map[x, y] = "g";
                        }
                        else
                        {
                            //Bottom wall
                            map[x, y] = "wb";
                        }
                    }
                }
            }
        }
    }
}

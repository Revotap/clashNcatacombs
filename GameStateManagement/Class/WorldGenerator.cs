using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Class
{
    internal class WorldGenerator
    {
        private int world_size;
        private bool[,] overworld;
        private char[,] actual_world;
        private int overworld_factor;

        public WorldGenerator(int world_size, int overworld_factor)
        {
            this.world_size = world_size;
            overworld = new bool[world_size,world_size];
            this.overworld_factor = overworld_factor;
        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Class
{
    internal class Key : Item
    {
        public Key(string name, int rarity, Texture2D texture, float rotation, int value) : base(name, texture, rarity, value, rotation) { }
    }
}

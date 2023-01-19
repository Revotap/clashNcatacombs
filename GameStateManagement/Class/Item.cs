using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Class
{
    internal class Item
    {
        public string name { get; set; }
        public Texture2D texture { get; set; }
        public int rarity { get; set; }
        public float rotation { get; set; }

        public Item(String name, Texture2D texture, int rarity, float rotation) {
            this.name = name;
            this.texture = texture;
            this.rarity = rarity;
            this.rotation = rotation;
        }
    }
}

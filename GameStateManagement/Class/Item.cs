using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Class
{
    internal abstract class Item
    {
        private String name;
        private Texture2D texture;
        private int textureResolution;
        private int rarity;
        private int id;

        public Item() { }

        public Texture2D Texture { get => texture; set => texture = value; }
        public int TextureResolution { get => textureResolution; set => textureResolution = value; }
        public string Name { get => name; set => name = value; }
        public int Rarity { get => rarity; set => rarity = value; }
        public int Id { get => id; set => id = value; }
    }
}

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
    internal abstract class Item
    {
        private String name;
        private Texture2D texture;
        private int textureResolution;
        private int rarity;
        private int id;

        // Properties
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public float Speed { get; set; }
        public float Rotation { get; set; }
        public Vector2 target { get; set; }

        public Vector2 originPosition { get; set; }

        public Item() { }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        public Texture2D Texture { get => texture; set => texture = value; }
        public int TextureResolution { get => textureResolution; set => textureResolution = value; }
        public string Name { get => name; set => name = value; }
        public int Rarity { get => rarity; set => rarity = value; }
        public int Id { get => id; set => id = value; }
    }
}

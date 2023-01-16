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
        
        public Key(String name, int rarity, Texture2D texture, int textureSize)
        {
            base.Name = name;
            base.Texture = texture;
            base.TextureResolution = textureSize;
            base.Rarity = rarity;

            if(name == "Silver Key")
            {
                base.Id = 1;
            }else if(name == "Golden key")
            {
                base.Id = 2;
            }else if(name == "Diamond Key")
            {
                base.Id = 3;
            }
        }
    }
}

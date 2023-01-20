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
        public float Speed { get; set; }
        public int value { get; }
        public bool isEquippable { get; }

        public Item(String name, Texture2D texture, int rarity, int value, float rotation) {
            this.name = name;
            this.texture = texture;
            this.rarity = rarity;
            this.rotation = rotation;
            this.isEquippable = false;
            this.value = value;
        }

        public Item(String name, Texture2D texture, int rarity, float rotation, int value, bool isEquippable)
        {
            this.name = name;
            this.texture = texture;
            this.rarity = rarity;
            this.rotation = rotation;
            this.isEquippable = isEquippable;
            this.value = value;
        }

        public Item(String name, Texture2D texture, int rarity, float rotation, int value, bool isEquippable, float speed)
        {
            this.name = name;
            this.texture = texture;
            this.rarity = rarity;
            this.rotation = rotation;
            this.isEquippable = isEquippable;
            this.Speed= speed;
            this.value = value;
        }
    }
}

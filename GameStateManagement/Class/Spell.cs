using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GameStateManagement.Class
{
    internal abstract class Spell: Item
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public float Speed { get; set; }
        public Vector2 targetPosition { get; set; }
        public Vector2 originPosition { get; set; }
        public Spell(String name, Texture2D texture, int rarity, float rotation, float speed) : base(name, texture, rarity, rotation) {
            this.Speed = speed;
        }

        protected Spell(String name, Texture2D texture, int rarity, float rotation, float speed, Vector2 position, Vector2 direction, Vector2 targetPosition, Vector2 originPosition) : base(name, texture, rarity, rotation) {
            this.Position= position;
            this.Direction= direction;
            this.Speed = speed;
            this.targetPosition= targetPosition;
            this.originPosition= originPosition;
            
        }

        public abstract void Update(GameTime gameTime);
        public abstract Spell Cast(Vector2 position, float rotation, Vector2 direction, Vector2 targetPosition, Vector2 originPosition);
    }
}

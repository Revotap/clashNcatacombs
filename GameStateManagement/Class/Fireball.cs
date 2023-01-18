using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Class
{
    internal class Fireball : Spell
    {
        // Constructor
        public Fireball(String name, Texture2D texture, int rarity, float rotation, float speed) : base(name, texture, rarity, rotation, speed) { }

        private Fireball(String name, Texture2D texture, int rarity, float rotation, float speed, Vector2 position, Vector2 direction, Vector2 targetPosition, Vector2 originPosition) : base(name, texture, rarity, rotation, speed, position, direction, targetPosition, originPosition) { }

        // Update method
        public override void Update(GameTime gameTime)
        {
            // Move the fireball in the direction and speed specified
            Position += Direction * Speed;
        }

        public override Spell Cast(Vector2 position, float rotation, Vector2 direction, Vector2 targetPosition, Vector2 originPosition)
        {
            return new Fireball(base.name, base.texture, base.rarity, rotation, base.Speed, position, direction, targetPosition, originPosition);
        }
    }
}

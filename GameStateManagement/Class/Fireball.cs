using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Class
{
    internal class Fireball : Item
    {
        // Constructor
        public Fireball(Texture2D texture, Vector2 position, Vector2 direction, float speed, float rotation, Vector2 originPosition, Vector2 target)
        {
            base.Texture = texture;
            base.Position = position;
            base.Direction = direction;
            base.Speed = speed;
            base.Rotation= rotation;
            base.originPosition= originPosition;
            base.target= target;
        }

        // Update method
        public override void Update(GameTime gameTime)
        {
            // Move the fireball in the direction and speed specified
            Position += Direction * Speed;
        }

        // Draw method
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}

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
    internal class Spell: Item
    {
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public Vector2 targetPosition { get; set; }
        public Vector2 originPosition { get; set; }

        public Character caster { get; set; }
        public Spell(String name, Texture2D texture, int rarity, float rotation, int value, float speed) : base(name, texture, rarity, rotation, value, true, speed)
        {

        }

        public Spell(String name, Texture2D texture, int rarity, float rotation, int value, float speed, Character caster) : base(name, texture, rarity, rotation, value, true, speed) {
            this.caster = caster;
        }

        protected Spell(String name, Texture2D texture, int rarity, float rotation, int value, float speed, Vector2 position, Vector2 direction, Vector2 targetPosition, Vector2 originPosition, Character caster) : base(name, texture, rarity, rotation,value, true, speed) {
            this.Position= position;
            this.Direction= direction;
            this.targetPosition= targetPosition;
            this.originPosition= originPosition;
            this.caster= caster;
        }

        public void Update(GameTime gameTime)
        {
            // Move the fireball in the direction and speed specified
            Position += Direction * Speed;
        }
        public Spell Cast(Vector2 position, float rotation, Vector2 direction, Vector2 targetPosition, Vector2 originPosition)
        {
            return new Spell(base.name, base.texture, base.rarity, rotation, base.value, Speed, position, direction, targetPosition, originPosition, caster);
        }
    }
}

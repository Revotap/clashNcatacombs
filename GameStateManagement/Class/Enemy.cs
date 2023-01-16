using GameStateManagement.Class;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Class
{
    internal class Enemy : Character
    {
        #region Variables
        private Item loot;
        private int exp;

        #endregion

        #region Properties
        public Item Loot { get => loot;}
        public int Exp { get => exp;}
        #endregion

        #region Constructor
        public Enemy(String name, List<Texture2D> enemyTexture, int width, int height, Item loot, int exp, int positionX, int positionY, int damage)
        {
            base.Name = name;
            base.TextureList = enemyTexture;
            base.Texture = TextureList.FirstOrDefault();
            this.loot = loot;
            this.exp = exp;
            base.PositionX= positionX;
            base.PositionY= positionY;
            base.BaseDamage= damage;

            base.Speed = 4f;
            base.Width = width;
            base.Height = height;
            base.BoundingBox = new Rectangle((int)base.PositionX, (int)base.PositionY + this.Height / 2, width, height / 2);
        }
        #endregion

        

        #region Methods
        public Item dropItem()
        {
            return Loot;
        }

        public int ExpGrant()
        {
            return Exp;
        }

        public override void moveUp()
        {
            throw new NotImplementedException();
        }

        public override void moveDown()
        {
            throw new NotImplementedException();
        }

        public override void moveLeft()
        {
            throw new NotImplementedException();
        }

        public override void moveRight()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            base.BoundingBoxX = (int)base.PositionX;
            base.BoundingBoxY = (int)base.PositionY + this.Height / 2;

            //Update der Animation
            TimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (TimeSinceLastFrame > MillisecondsPerFrame)
            {
                TimeSinceLastFrame -= MillisecondsPerFrame;
                base.Texture = TextureList.ElementAt(NextTexture);
                NextTexture++;
                if (NextTexture >= TextureList.Count)
                {
                    NextTexture = 0;
                }
            }
        }

        public override void doDamage(int damage)
        {
            if (DamageReceivedSound != null)
            {
                DamageReceivedSound.Play();
            }
            
            this.HealthPoints -= damage;
        }

        public override void attack(GameTime gameTime, Character target)
        {
            TimeSinceLastAttack += gameTime.ElapsedGameTime.Milliseconds;
            if (TimeSinceLastAttack > AttackSpeed)
            {
                TimeSinceLastAttack -= AttackSpeed;
                if(AttackSound != null)
                {
                    AttackSound.Play();
                }
                target.doDamage(BaseDamage);
            }
        }

        //Getter and Setter
        public new Texture2D Texture { get => base.Texture; set => base.Texture = value; }
        public new Vector2 Position { get => base.Position; set => base.Position = value; }
        public new Rectangle BoundingBox { get => base.BoundingBox; }

        #endregion

    }


}
 


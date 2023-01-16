using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Class
{
    internal class Player : Character
    {
        #region Variables
        Tile interactableNearby;
        Inventory playerInventory;
        #endregion

        #region Construktor
        public Player(String name, List<Texture2D> playerTexture, int width, int height)
        {
            base.Name = name;
            base.TextureList = playerTexture;
            base.Texture = TextureList.FirstOrDefault();

            //Debugging
            base.T_class = "Knight";
            base.HealthPoints = 6;
            base.Speed = 4f;

            base.Width = width;
            base.Height = height;
            base.BoundingBox = new Rectangle((int) base.PositionX, (int) base.PositionY + this.Height/2, width, height/2);
            this.PlayerInventory= new Inventory();
        }
        #endregion

        #region Methods
        public override void moveUp()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                base.PositionY -= base.Speed;
            }
        }
        public override void moveDown()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                base.PositionY += base.Speed;
            }
        }
        public override void moveLeft()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                base.PositionX -= base.Speed;
            }
        }
        public override void moveRight()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                base.PositionX += base.Speed;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.BoundingBoxX = (int) base.PositionX;
            base.BoundingBoxY = (int) base.PositionY + this.Height/2;

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
            if(this.HealthPoints <= 0)
            {
                return;
            }

            if (DamageReceivedSound != null)
            {
                DamageReceivedSound.Play();
            }
            this.HealthPoints -= damage;

            if (this.HealthPoints <= 0)
            {
                if (DeathSound != null)
                {
                    DeathSound.Play();
                }
            }
        }

        public override void attack(GameTime gameTime, Character target)
        {
            TimeSinceLastAttack += gameTime.ElapsedGameTime.Milliseconds;
            if (TimeSinceLastAttack > AttackSpeed)
            {
                TimeSinceLastAttack -= AttackSpeed;
                if (AttackSound != null)
                {
                    AttackSound.Play();
                }
                target.doDamage(10);
            }
        }

        //Getter and Setter
        public new Texture2D Texture { get => base.Texture; set => base.Texture = value; }
        public new Vector2 Position { get => base.Position; set => base.Position = value; }
        #endregion
        public new Rectangle BoundingBox { get => base.BoundingBox; }
        internal Tile InteractableNearby { get => interactableNearby; set => interactableNearby = value; }
        internal Inventory PlayerInventory { get => playerInventory; set => playerInventory = value; }
    }
}

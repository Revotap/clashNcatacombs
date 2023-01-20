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
    internal class Player : Character
    {
        #region Variables
        public Tile interactableNearby { get; set; }
        public Inventory inventory { get; set; }
        private int currentXP = 0;
        private int maxXPForCurrentLevel = 10;
        private int level = 1;

        #endregion

        #region Construktor
        public Player(String name, int health, int width, int height, Vector2 position, List<Texture2D> textures, float movementSpeed, SoundEffect damageReceivedSound, SoundEffect deathSound, SoundEffect attackWithNoWeaponSound, Inventory inventory) : base(name, health, width, height, position, textures, movementSpeed, damageReceivedSound, deathSound, attackWithNoWeaponSound)
        {
            this.inventory = inventory;
        }


        #endregion
        #region Methods
        public override void updatePosition(List<TileEntry> collisionObjects)
        {
            foreach(TileEntry item in collisionObjects)
            {
                if(this.velocity.X > 0 && this.isTouchingRight(item.boundingBox) || this.velocity.X < 0 && this.isTouchingLeft(item.boundingBox)){
                    velocity = new Vector2(0, velocity.Y);
                }

                if(this.velocity.Y < 0 && this.isTouchingUp(item.boundingBox) || this.velocity.Y > 0 && this.isTouchingDown(item.boundingBox))
                {
                    velocity = new Vector2(velocity.X, 0);
                }
            }
            position += velocity;
            velocity = Vector2.Zero;
        }
        
        public override void moveUp()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                velocity = new Vector2(velocity.X, velocity.Y - movementSpeed);
            }
        }
        public override void moveDown()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                velocity = new Vector2(velocity.X, velocity.Y + movementSpeed);
            }
        }
        public override void moveLeft()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                velocity = new Vector2(velocity.X - movementSpeed, velocity.Y);
            }
        }
        public override void moveRight()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                velocity = new Vector2(velocity.X + movementSpeed, velocity.Y);
            }
        }

        public override bool isTouchingLeft(Rectangle rect)
        {
            return rect.Right > boundingBox.Left + velocity.X &&
                rect.Left < boundingBox.Left &&
                rect.Bottom > boundingBox.Top &&
                rect.Top < boundingBox.Bottom;
        }

        public override bool isTouchingRight(Rectangle rect)
        {
            return rect.Left < boundingBox.Right + velocity.X &&
                rect.Right > boundingBox.Right &&
                rect.Bottom > boundingBox.Top &&
                rect.Top < boundingBox.Bottom;
        }

        public override bool isTouchingUp(Rectangle rect)
        {
            return rect.Bottom > boundingBox.Top + velocity.Y &&
                rect.Top < boundingBox.Top &&
                rect.Right > boundingBox.Left &&
                rect.Left < boundingBox.Right;
        }

        public override bool isTouchingDown(Rectangle rect)
        {
            return rect.Top < boundingBox.Bottom + velocity.Y &&
                rect.Bottom > boundingBox.Bottom &&
                rect.Right > boundingBox.Left &&
                rect.Left < boundingBox.Right;
        }

        public override void Update(GameTime gameTime)
        {
            boundingBox.X = (int) position.X;
            boundingBox.Y = (int) position.Y + height/2;

            //Update der Animation
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > frameSpeed)
            {
                timeSinceLastFrame -= frameSpeed;
                nextTexture++;
                if (nextTexture >= textures.Count)
                {
                    nextTexture = 0;
                }
            }
        }

        public override void receiveDamage(Character source, int damage)
        {
            if(health <= 0)
            {
                return;
            }

            if (damageReceivedSound != null)
            {
                damageReceivedSound.Play();
            }
            this.health -= damage;

            if (this.health <= 0)
            {
                if (deathSound != null)
                {
                    deathSound.Play();
                }
            }
        }

        public override void attack(GameTime gameTime, Character target)
        {
            timeSinceLastAttack += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastAttack > attackSpeed)
            {
                timeSinceLastAttack -= attackSpeed;
                if (attackWithNoWeaponSound != null)
                {
                    attackWithNoWeaponSound.Play();
                }
                target.receiveDamage(this, 10);
            }
        }

        public int getCurrentXP() { return currentXP; }
        public override void addXP(int xp) {
            currentXP += xp;
            if(currentXP >= maxXPForCurrentLevel)
            {
                level++;
                currentXP = currentXP - maxXPForCurrentLevel;
                maxXPForCurrentLevel = maxXPForCurrentLevel * level;
            }
        }
        public int getMaxXForCurrentLevel() { return maxXPForCurrentLevel; }
        public int getLevel() { return level; }

        public void calculatePushBack(Rectangle rect, List<TileEntry> collisionObjects)
        {
            //bool tmpBoolLeft = this.isTouchingLeft(rect);
            //bool tmpBoolRight = this.isTouchingRight(rect);
            //bool tmpBoolUp = this.isTouchingUp(rect);
            //bool tmpBoolDown = this.isTouchingDown(rect);
            //throw new Exception(tmpBoolLeft.ToString() + tmpBoolRight + tmpBoolUp + tmpBoolDown + tmpVelocity.X + tmpVelocity.Y);
            if (velocity.X > 0 && this.isTouchingRight(rect) || velocity.X < 0 && this.isTouchingLeft(rect))
            {
                velocity = new Vector2(velocity.X * (-10), velocity.Y);
            }

            if (velocity.Y < 0 && this.isTouchingUp(rect) || velocity.Y > 0 && this.isTouchingDown(rect))
            {
                velocity = new Vector2(velocity.X, velocity.Y * (-10));
            }
            foreach (TileEntry item in collisionObjects)
            {
                if (velocity.X > 0 && this.isTouchingRight(item.boundingBox) || velocity.X < 0 && this.isTouchingLeft(item.boundingBox))
                {
                    velocity = new Vector2(0, velocity.Y);
                }

                if (velocity.Y < 0 && this.isTouchingUp(item.boundingBox) || velocity.Y > 0 && this.isTouchingDown(item.boundingBox))
                {
                    velocity = new Vector2(velocity.X, 0);
                }
            }
            position += velocity;
            velocity = Vector2.Zero;
        }
        #endregion
    }
}

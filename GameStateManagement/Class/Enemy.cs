using GameStateManagement.Class;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameStateManagement.Class
{
    internal class Enemy : Character
    {
        #region Variables
        private List<Item> loot_table;
        private int exp;
        
        //Not in use currently
        private Vector2 direction;
        private float rotation;

        private Random random = new Random();
        int traveledInSameDirection = 0;
        int lastDirection;
        #endregion

        #region Constructor
        public Enemy(String name, int health, int width, int height, Vector2 position, List<Texture2D> textures, float movementSpeed, SoundEffect damageReceivedSound, SoundEffect deathSound, SoundEffect attackWithNoWeaponSound, List<Item> loot, int exp) : base (name, health, width, height, position, textures, movementSpeed, damageReceivedSound, deathSound, attackWithNoWeaponSound)
        {
            this.loot_table = loot;
            this.exp = exp;
            lastDirection = random.Next(0, 4);
        }

        public Enemy(String name, int health, int width, int height, Vector2 position, List<Texture2D> textures, float movementSpeed, SoundEffect damageReceivedSound, SoundEffect deathSound, SoundEffect attackWithNoWeaponSound, List<Item> loot, int exp, Spell equippedItem) : base(name, health, width, height, position, textures, movementSpeed, damageReceivedSound, deathSound, attackWithNoWeaponSound)
        {
            this.loot_table = loot;
            this.exp = exp;
            base.equiptedItem = equippedItem;
            lastDirection = random.Next(0, 4);
        }
        #endregion

        #region Methods
        public Item dropItem()
        {
            Item selectedItem = loot_table[new Random().Next(0, loot_table.Count)];
            if(selectedItem is Spell)
            {
                return new Spell(selectedItem.name, selectedItem.texture, selectedItem.rarity, selectedItem.rotation, selectedItem.value, selectedItem.Speed);
            }
            else
            {
                return new Item(selectedItem.name, selectedItem.texture, selectedItem.rarity, selectedItem.value, selectedItem.rotation);
            }
        }

        public int ExpGrant()
        {
            return exp;
        }

        public int getNextMove()
        {
            //caclculate random movement based on random numbers and times
            if(traveledInSameDirection < random.Next(300,600) && lastDirection == 0)
            {
                traveledInSameDirection++;
                return 0;
            }
            else if(traveledInSameDirection < random.Next(300, 600) && lastDirection == 1)
            {
                traveledInSameDirection++;
                return 1;
            }
            else if (traveledInSameDirection < random.Next(300, 600) && lastDirection == 2)
            {
                traveledInSameDirection++;
                return 2;
            }
            else if (traveledInSameDirection < random.Next(300, 600) && lastDirection == 3)
            {
                traveledInSameDirection++;
                return 3;
            }
            traveledInSameDirection= 0;
            int tmpNum = random.Next(0, 4);
            lastDirection= tmpNum;
            return tmpNum;
        }

        public int getNextMove(Vector2 target)
        {
            return 0;
        }

        public override void moveUp()
        {
            velocity = new Vector2(velocity.X, velocity.Y - movementSpeed);
        }
        public override void moveDown()
        {
            velocity = new Vector2(velocity.X, velocity.Y + movementSpeed);
        }
        public override void moveLeft()
        {
            velocity = new Vector2(velocity.X - movementSpeed, velocity.Y);
        }
        public override void moveRight()
        {
            velocity = new Vector2(velocity.X + movementSpeed, velocity.Y);
        }

        public override void Update(GameTime gameTime)
        {
            base.boundingBox.X = (int)position.X;
            base.boundingBox.Y = (int)position.Y + height / 2;

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
            if (damageReceivedSound != null)
            {
                damageReceivedSound.Play();
            }
            
            health -= damage;

            if (this.health <= 0)
            {
                if (deathSound != null)
                {
                    deathSound.Play();
                }
                source.addXP(exp);
            }
        }

        public override void attack(GameTime gameTime, Character target)
        {
            timeSinceLastAttack += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastAttack > attackSpeed)
            {
                timeSinceLastAttack -= attackSpeed;
                if(attackWithNoWeaponSound != null)
                {
                    attackWithNoWeaponSound.Play();
                }
                target.receiveDamage(this, baseDamage);
            }
        }

        public void attackWithSpell(GameTime gameTime, Character target,Vector3 cameraPos, List<Spell> casted_spells)
        {
            timeSinceLastAttack += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastAttack > attackSpeed)
            {
                timeSinceLastAttack -= attackSpeed;
                if (attackWithNoWeaponSound != null)
                {
                    attackWithNoWeaponSound.Play();
                }

                Vector2 originPosition = new Vector2(position.X + width/2, position.Y + height/2);

                List<Vector2> targetVectors = new List<Vector2>();               

                //ziel - standort
                targetVectors.Add(new Vector2(position.X + 1, position.Y - 100));
                targetVectors.Add(new Vector2(position.X + 1, position.Y + 100));
                targetVectors.Add(new Vector2(position.X - 100, position.Y + 1));
                targetVectors.Add(new Vector2(position.X + 100, position.Y + 1));
                targetVectors.Add(new Vector2(position.X + 100, position.Y - 100));
                targetVectors.Add(new Vector2(position.X - 100, position.Y + 100));
                targetVectors.Add(new Vector2(position.X - 100, position.Y - 100));
                targetVectors.Add(new Vector2(position.X + 100, position.Y + 100));

                foreach (Vector2 targetVector in targetVectors)
                {
                    // Get the direction from the player to the mouse
                    Vector2 spellDirection = new Vector2(targetVector.X - position.X, targetVector.Y - position.Y);
                    spellDirection.Normalize();

                    float rotation = (float)Math.Atan2(spellDirection.X, spellDirection.Y);

                    // Create a new spell at the player's position
                    Spell spell = new Spell(EquiptedItem().name, EquiptedItem().texture, EquiptedItem().rarity, rotation, EquiptedItem().value, EquiptedItem().Speed, this);
                    casted_spells.Add(spell.Cast(originPosition, rotation, spellDirection, targetVector, originPosition));
                }
            }
        }

        //Not in use currently
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

        public override void updatePosition(List<TileEntry> collisionObjects)
        {
            foreach (TileEntry item in collisionObjects)
            {
                if (this.velocity.X > 0 && this.isTouchingRight(item.boundingBox) || this.velocity.X < 0 && this.isTouchingLeft(item.boundingBox))
                {
                    velocity = new Vector2(0, velocity.Y);
                }

                if (this.velocity.Y < 0 && this.isTouchingUp(item.boundingBox) || this.velocity.Y > 0 && this.isTouchingDown(item.boundingBox))
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
 


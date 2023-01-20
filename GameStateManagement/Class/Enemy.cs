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
        #endregion

        #region Constructor
        public Enemy(String name, int health, int width, int height, Vector2 position, List<Texture2D> textures, float movementSpeed, SoundEffect damageReceivedSound, SoundEffect deathSound, SoundEffect attackWithNoWeaponSound, List<Item> loot, int exp) : base (name, health, width, height, position, textures, movementSpeed, damageReceivedSound, deathSound, attackWithNoWeaponSound)
        {
            this.loot_table = loot;
            this.exp = exp;
        }

        public Enemy(String name, int health, int width, int height, Vector2 position, List<Texture2D> textures, float movementSpeed, SoundEffect damageReceivedSound, SoundEffect deathSound, SoundEffect attackWithNoWeaponSound, List<Item> loot, int exp, Spell equippedItem) : base(name, health, width, height, position, textures, movementSpeed, damageReceivedSound, deathSound, attackWithNoWeaponSound)
        {
            this.loot_table = loot;
            this.exp = exp;
            base.equiptedItem = equippedItem;
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

        public override void updatePosition(List<TileEntry> collisionObjects)
        {
            throw new NotImplementedException();
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
        public void moveTo(Vector2 position)
        {
        }
        public override bool isTouchingLeft(Rectangle item)
        {
            throw new NotImplementedException();
        }
        public override bool isTouchingRight(Rectangle item)
        {
            throw new NotImplementedException();
        }
        public override bool isTouchingUp(Rectangle item)
        {
            throw new NotImplementedException();
        }
        public override bool isTouchingDown(Rectangle item)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
 


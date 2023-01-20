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
 


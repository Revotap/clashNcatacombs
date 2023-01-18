using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Class
{
    internal class Tile
    {
        private List<Texture2D> textures;

        public Texture2D texture()
        {
            if(textures.Count > 0)
            {
                return textures[nextTexture];
            }
            else
            {
                return null;
            }
        }

        protected bool hasCollision { get; set; } = false;
        public bool getHasCollision() { return hasCollision; }
        protected bool isInteractable { get; set; } = false;
        public bool getIsInteractable() { return isInteractable; }
        protected bool interacted { get; set; } = false;
        protected bool isLocked { get; set; } = false;
        protected bool doesDamage { get; set; } = false;
        public bool getDoesDamage() { return doesDamage; }
        protected bool isAnimated { get; set; } = false;

        private Texture2D interactedTexture;
        private Tile neightbourInteractable;
        protected SoundEffect interactionSound { get; set; }

        private Item requiredItem { get; set; }
        public Item getRequiredItem() { return requiredItem; }

        private int baseDamage = 1;
        private SoundEffect attackSound;
        private int attackSpeed = 700;

        private int frameSpeed = 400;
        private SoundEffect animationSound;

        private int timeSinceLastAttack = 0;
        private int timeSinceLastFrame = 0;
        private int nextTexture = 0;

        public Tile(Texture2D textures, bool hasCollision)
        {
            this.textures = new List<Texture2D>();
            this.textures.Add(textures);
            this.hasCollision = hasCollision;
        }

        public void SetIsInteractable(Texture2D interactedTexture, Tile neighbourInteractable, SoundEffect interactionSound)
        {
            this.isInteractable = true;
            this.interactedTexture = interactedTexture;
            this.neightbourInteractable= neighbourInteractable;
            this.interactionSound= interactionSound;
        }

        public void SetIsLocked(Item requiredItem)
        {
            this.isLocked = true;
            this.requiredItem= requiredItem;
        }

        public void SetDoesDamage(int baseDamage, SoundEffect attackSound, int attackSpeed)
        {
            this.doesDamage = true;
            this.baseDamage = baseDamage;
            this.attackSound= attackSound;
            this.attackSpeed = attackSpeed;
        }

        public void SetIsAnimated(List<Texture2D> animation, int frameSpeed, SoundEffect animationSound)
        {
            this.isAnimated= true;
            this.textures = animation;
            this.frameSpeed= frameSpeed;
            this.animationSound= animationSound;
        }

        //Methods
        public void Update(GameTime gameTime)
        {
            if (this.isAnimated)
            {
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame > frameSpeed)
                {
                    timeSinceLastFrame -= frameSpeed;
                    nextTexture++;
                    if (nextTexture >= textures.Count-1)
                    {
                        nextTexture = 0;
                    }
                }
            }
            
        }

        protected void Interact()
        {
            if (this.isInteractable)
            {
                isLocked = false;
                interacted = true;
                isInteractable = false;
                this.hasCollision = false;
                this.textures.Clear();
                this.textures.Add(interactedTexture);
                nextTexture = 0;
            }
        }

        public virtual Item Interact(Inventory inventory)
        {
            if (interacted)
            {
                return null;
            }
            if (isLocked)
            {
                //Key required
                foreach (Item i in inventory.Item_list)
                {
                    if (i != null)
                    {
                        if (i.Id == requiredItem.Id)
                        {
                            inventory.RemoveItem(i);
                            isLocked = false;

                            if (neightbourInteractable != null)
                            {
                                neightbourInteractable.Interact();
                            }
                            if (interactionSound != null)
                            {
                                interactionSound.Play();
                            }
                            return Interact(inventory);
                        }
                    }
                }
                return null;
            }
            else //No Key required
            {
                this.Interact();
                if (neightbourInteractable != null)
                {
                    neightbourInteractable.Interact();
                }
                if (interactionSound != null)
                {
                    interactionSound.Play();
                }

                return null;
            }
        }

        public void attack(GameTime gameTime, Character target)
        {
            timeSinceLastAttack += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastAttack > attackSpeed)
            {
                timeSinceLastAttack -= attackSpeed;
                if (attackSound != null)
                {
                    attackSound.Play();
                }
                target.doDamage(baseDamage);
            }
        }
    }
}

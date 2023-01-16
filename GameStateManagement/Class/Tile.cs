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
        private Texture2D texture;
        private int textureResolution;
        private Vector2 textureVector;

        private Texture2D interactedTexture;
        private Vector2 interactedTextureVector;
        
        private bool hasCollision;
        private bool isInteractable;
        private bool interacted = false;

        private bool isLocked = false;
        private Key requiredItem;

        private Tile neighborInteractable = null;
        private SoundEffect interactionSound;

        private bool doesDamage = false;

        private int timeSinceLastAttack = 0;
        private int attackSpeed = 700;
        private int baseDamage = 1;
        private SoundEffect attackSound;

        public Tile(Texture2D texture, int textureResolution, Vector2 textureVector, bool hasCollision, bool doesDamage)
        {
            this.Texture = texture;
            this.TextureResolution = textureResolution;
            this.TextureVector = textureVector;
            this.HasCollision = hasCollision;

            this.interactedTextureVector = textureVector;
            this.interactedTexture = texture;
            this.doesDamage = doesDamage;
        }

        public Tile(Texture2D texture, int textureResolution, Vector2 textureVector, bool hasCollision, bool isInteractable, SoundEffect sound, bool doesDamage)
        {
            this.Texture = texture;
            this.TextureResolution = textureResolution;
            this.TextureVector = textureVector;
            this.HasCollision = hasCollision;
            this.isInteractable = isInteractable;

            this.interactedTextureVector = textureVector;
            this.interactedTexture = texture;

            this.interactionSound= sound;
            this.doesDamage = doesDamage;
        }

        public Tile(Texture2D texture, int textureResolution, Vector2 textureVector, bool hasCollision, bool isInteractable, Tile interactedTextureTile, SoundEffect sound, bool doesDamage)
        {
            this.Texture = texture;
            this.TextureResolution = textureResolution;
            this.TextureVector = textureVector;
            this.HasCollision = hasCollision;
            this.isInteractable = isInteractable;

            this.interactedTextureVector = interactedTextureTile.textureVector;
            this.interactedTexture = interactedTextureTile.texture;

            this.interactionSound = sound;
            this.doesDamage = doesDamage;
        }

        public Tile(Texture2D texture, int textureResolution, Vector2 textureVector, bool hasCollision, bool isInteractable, Tile interactedTextureTile, bool isLocked, Key requiredItem, SoundEffect sound, bool doesDamage)
        {
            this.Texture = texture;
            this.TextureResolution = textureResolution;
            this.TextureVector = textureVector;
            this.HasCollision = hasCollision;
            this.isInteractable = isInteractable;

            this.interactedTextureVector = interactedTextureTile.textureVector;
            this.interactedTexture = interactedTextureTile.texture;

            this.isLocked= isLocked;
            this.requiredItem = requiredItem;

            this.interactionSound = sound;
            this.doesDamage = doesDamage;
        }

        public Tile(Texture2D texture, int textureResolution, Vector2 textureVector, bool hasCollision, bool isInteractable, Vector2 interactedTextureVector, SoundEffect sound, bool doesDamage)
        {
            this.Texture = texture;
            this.TextureResolution = textureResolution;
            this.TextureVector = textureVector;
            this.HasCollision = hasCollision;
            this.isInteractable = isInteractable;

            this.interactedTextureVector = interactedTextureVector;
            this.interactedTexture = texture;

            this.interactionSound = sound;
            this.doesDamage = doesDamage;
        }

        public Tile(Texture2D texture, int textureResolution, Vector2 textureVector, bool hasCollision, bool isInteractable, Vector2 interactedTextureVector, bool isLocked, Key requiredItem, SoundEffect sound, bool doesDamage)
        {
            this.Texture = texture;
            this.TextureResolution = textureResolution;
            this.TextureVector = textureVector;
            this.HasCollision = hasCollision;
            this.isInteractable = isInteractable;

            this.interactedTextureVector = interactedTextureVector;
            this.interactedTexture = texture;

            this.isLocked = isLocked;
            this.requiredItem = requiredItem;

            this.interactionSound = sound;
            this.doesDamage = doesDamage;
        }

        public void interarctAsNeighbor()
        {
            if(this.isInteractable)
            {
                isLocked = false;
                interacted = true;
                isInteractable = false;
                this.hasCollision = false;
                this.textureVector = interactedTextureVector;
                this.texture = interactedTexture;
            }
        }

        public virtual Item Interact(Inventory inventory)
        {
            if (interacted)
            {
                return null;
            }
            if(isLocked)
            {
                //Key required
                foreach(Item i in inventory.Item_list)
                {
                    if (i != null)
                    {
                        if (i.Id == requiredItem.Id)
                        {
                            inventory.RemoveItem(i);
                            isLocked = false;
                            neighborInteractable.interarctAsNeighbor();
                            if(interactionSound != null)
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
                interacted = true;
                isInteractable = false;
                this.hasCollision = false;
                this.textureVector = interactedTextureVector;
                this.texture = interactedTexture;

                return null;
            }            
        }

        public void doDamage(GameTime gameTime, Character target)
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

        public Texture2D Texture { get => texture; set => texture = value; }
        public int TextureResolution { get => textureResolution; set => textureResolution = value; }
        public Vector2 TextureVector { get => textureVector; set => textureVector = value; }
        public bool HasCollision { get => hasCollision; set => hasCollision = value; }
        public bool IsInteractable { get => isInteractable; set => isInteractable = value; }
        public bool Interacted { get => interacted; set => interacted = value; }
        internal Texture2D InteractedTexture { get => interactedTexture; set => interactedTexture = value; }
        public Vector2 InteractedTextureVector { get => interactedTextureVector; set => interactedTextureVector = value; }
        public bool IsLocked { get => isLocked; set => isLocked = value; }
        internal Tile NeighborInteractable { get => neighborInteractable; set => neighborInteractable = value; }
        public Key RequiredItem { get => requiredItem; set => requiredItem = value; }
        public bool DoesDamage { get => doesDamage; set => doesDamage = value; }
    }
}

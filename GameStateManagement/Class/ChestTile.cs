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
    internal class ChestTile : Tile
    {
        private List<Item> loot;
        private SoundEffect interactSound;
        public ChestTile(Texture2D texture, int textureResolution, Vector2 textureVector, bool hasCollision, bool isInteractable, SoundEffect sound, bool doesDamage) : base(texture, textureResolution, textureVector, hasCollision, isInteractable, sound, doesDamage)
        {
            this.loot = new List<Item>();
        }

        public ChestTile(Texture2D texture, int textureResolution, Vector2 textureVector, bool hasCollision, bool isInteractable, Vector2 interactedTextureVector, Item loot, SoundEffect sound, bool doesDamage) : base(texture, textureResolution, textureVector, hasCollision, isInteractable, interactedTextureVector, sound, doesDamage)
        {
            this.loot = new List<Item>();
            this.loot.Add(loot);
        }

        public ChestTile(Texture2D texture, int textureResolution, Vector2 textureVector, bool hasCollision, bool isInteractable, Tile interactedTextureTile, Item loot, SoundEffect sound, bool doesDamage) : base(texture, textureResolution, textureVector, hasCollision, isInteractable, interactedTextureTile, sound, doesDamage)
        {
            this.loot = new List<Item>();
            this.loot.Add(loot);
            this.interactSound= sound;
        }

        public ChestTile(Texture2D texture, int textureResolution, Vector2 textureVector, bool hasCollision, bool isInteractable, Item loot, SoundEffect sound, bool doesDamage) : base(texture, textureResolution, textureVector, hasCollision, isInteractable, sound, doesDamage)
        {
            this.loot = new List<Item>();
            this.loot.Add(loot);
            this.interactSound= sound;
        }

        public override Item Interact(Inventory inventory)
        {
            if (base.Interacted) return null;

            base.Interact(inventory);

            if (loot.Count> 0)
            {
                //Returns random loot from Loottable
                //return new Key("loot from table", 0, null, 16);
                if(interactSound != null)
                {
                    interactSound.Play();
                }
                return loot[new Random().Next(0, loot.Count)];
            }
            else
            {
                return null;
            }
        }
    }
}

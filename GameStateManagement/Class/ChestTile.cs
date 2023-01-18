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

        public ChestTile(Texture2D texture, bool hasCollision, List<Item> loot) : base(texture, hasCollision)
        {
            this.loot = loot;
        }

        public override Item Interact(Inventory inventory)
        {
            if (base.interacted) return null;
            
            base.Interact(inventory);
            
            if (loot.Count> 0)
            {
                //Returns random loot from Loottable
                //return new Key("loot from table", 0, null, 16);
                if(base.interactionSound != null)
                {
                    base.interactionSound.Play();
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

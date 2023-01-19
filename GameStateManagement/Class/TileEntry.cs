using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Class
{
    internal class TileEntry
    {
        public Tile tile { get; }
        public Vector2 drawVector { get; set; }
        private int targetTextureResolution;
        public Rectangle boundingBox { get; }

        public TileEntry(Tile tile, Vector2 drawVector, int targetTextureResolution)
        {
            this.tile = tile;
            this.drawVector = drawVector;
            this.targetTextureResolution = targetTextureResolution;

            if (tile.getHasCollision() || tile.getIsInteractable() || tile.getDoesDamage())
            {
                this.boundingBox = new Rectangle((int)drawVector.X, (int)drawVector.Y, targetTextureResolution, targetTextureResolution);
            }    
        }
        public bool hit(Vector2 targetVector)
        {
            if (targetVector.X > boundingBox.X && targetVector.X < (boundingBox.X + boundingBox.Height) && targetVector.Y > boundingBox.Y && targetVector.Y < (boundingBox.Y + boundingBox.Width))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

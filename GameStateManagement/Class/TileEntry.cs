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
        private Tile tile;
        private Vector2 drawVector;
        private int targetTextureResolution;
        private Rectangle boundingBox;

        public TileEntry(Tile tile, Vector2 drawVector, int targetTextureResolution)
        {
            this.Tile = tile;
            this.DrawVector = drawVector;
            this.targetTextureResolution = targetTextureResolution;

            if (tile.HasCollision || tile.IsInteractable)
            {
                this.boundingBox = new Rectangle((int)drawVector.X, (int)drawVector.Y, targetTextureResolution, targetTextureResolution);
            }    
        }

        public Vector2 DrawVector { get => drawVector; set => drawVector = value; }
        public Rectangle BoundingBox { get => boundingBox; set => boundingBox = value; }
        internal Tile Tile { get => tile; set => tile = value; }
    }
}

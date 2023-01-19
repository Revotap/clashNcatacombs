using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameStateManagement.Class
{
    internal class Inventory
    {
        public List<Item> item_list { get; set; }
        public int inventorySlots { get; } = 8;

        private Vector2 drawVector;
        private Texture2D empty_texture;
        private Texture2D selected_texture;
        private int selected_item_id = 0;


        public Inventory(Vector2 drawVector, Texture2D empty_texture, Texture2D selected_texture) {
            item_list = new List<Item>();
            this.drawVector = drawVector;
            this.empty_texture = empty_texture;
            this.selected_texture = selected_texture;
        }

        public bool invenotryFull()
        {
            if (item_list.Count >= inventorySlots)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool AddItem(Item item) {
            if(item != null)
            {
                if (item_list.Count < inventorySlots)
                {
                    item_list.Add(item);
                    //throw new Exception();
                    return true;
                }
            }
            return false;
        }

        public bool RemoveItem(Item item)
        {
            if(item_list.Contains(item))
            {
                item_list.Remove(item);
                return true;
            }
            else
            {
                return false;
            }
        }

        public String GetItemName(int index)
        {
            if (item_list.Count > index && item_list[index] != null) {
                return item_list[index].name;
            }
            else
            {
                return "Empty";
            }
        }

        public bool RemoveItemWithIndex(int index)
        {
            if (item_list[index] != null)
            {
                item_list.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void selectItemNumber(int index)
        {
            selected_item_id = index;
        }

        public void Draw(SpriteBatch _spriteBatch, SpriteFont spriteFont, Vector3 cameraPos, int targetTextureResolution)
        {
            int tmp = 0;
            foreach(Item item in item_list)
            {
                if (selected_item_id == tmp)
                {
                    _spriteBatch.DrawString(spriteFont, item.name, new Vector2(drawVector.X + 200 - cameraPos.X, drawVector.Y - cameraPos.Y - 40), Color.White);
                    _spriteBatch.Draw(selected_texture, new Rectangle((int)drawVector.X + (targetTextureResolution * tmp) - (int)cameraPos.X, (int)drawVector.Y - (int)cameraPos.Y, targetTextureResolution, targetTextureResolution), Color.White);
                }
                else
                {
                    _spriteBatch.Draw(empty_texture, new Rectangle((int)drawVector.X + (targetTextureResolution * tmp) - (int)cameraPos.X, (int)drawVector.Y - (int)cameraPos.Y, targetTextureResolution, targetTextureResolution), Color.White);
                }
                tmp++;
            }
            for(; tmp < inventorySlots; tmp++)
            {
                if (selected_item_id == tmp)
                {
                    if(selected_item_id < item_list.Count)
                    {
                        _spriteBatch.DrawString(spriteFont, item_list[tmp].name, new Vector2(drawVector.X + 200 - cameraPos.X, drawVector.Y - cameraPos.Y - 40), Color.White);
                    }
                    _spriteBatch.Draw(selected_texture, new Rectangle((int)drawVector.X + (targetTextureResolution * tmp) - (int)cameraPos.X, (int)drawVector.Y - (int)cameraPos.Y, targetTextureResolution, targetTextureResolution), Color.White);
                }
                else
                {
                    _spriteBatch.Draw(empty_texture, new Rectangle((int)drawVector.X + (targetTextureResolution * tmp) - (int)cameraPos.X, (int)drawVector.Y - (int)cameraPos.Y, targetTextureResolution, targetTextureResolution), Color.White);
                }
            }

            tmp = 0;
            foreach(Item item in item_list)
            {
                _spriteBatch.Draw(item.texture, new Rectangle((int)drawVector.X + (targetTextureResolution * tmp) - (int)cameraPos.X, (int)drawVector.Y - (int)cameraPos.Y, targetTextureResolution, targetTextureResolution), Color.White);
                tmp++;
            }
        }
    }
}

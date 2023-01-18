using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Class
{
    internal class Inventory
    {
        public List<Item> item_list { get; set; }
        public int inventorySlots { get; } = 4;


        public Inventory() {
            item_list = new List<Item>();
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
    }
}

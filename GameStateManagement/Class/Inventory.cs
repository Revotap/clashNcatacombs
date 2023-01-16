using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStateManagement.Class
{
    internal class Inventory
    {
        private List<Item> item_list;
        private int slots_x = 2;
        private int slots_y = 2;


        public Inventory() {
            item_list = new List<Item>();

            //Testing
            //
            //item_list.Add(new Key("Silver Key", 0, null, 16));
        }

        public int Slots_x { get => slots_x; set => slots_x = value; }
        public int Slots_y { get => slots_y; set => slots_y = value; }
        internal List<Item> Item_list { get => item_list; set => item_list = value; }

        public bool AddItem(Item item) {
            //throw new Exception();
            if(item != null)
            {
                if (item_list.Count < (slots_x * slots_y))
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
                return item_list[index].Name;
            }
            else
            {
                return "Empty";
            }
        }

    }
}

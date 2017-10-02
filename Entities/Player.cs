using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppBZork.Entities
{
    class Player
    {
        public string Name { get; set; }
        public static Dictionary<string, Item> inventory = new Dictionary<string, Item>();
        public static Dictionary<string, Item> tempInventory = new Dictionary<string, Item>();

        public static void ShowInventory()
        {
            if (inventory.Count() == 0)
            {
                Console.WriteLine($"There's nothing in your inventory.");
                return;
            }
            int count = 1;
            Console.Write($"You have: ");
            foreach (var item in inventory)
            {

                if (count == inventory.Count())
                {
                    Console.Write($"{item.Value.Name} ");
                    Console.WriteLine($"in your inventory!");
                    return;
                }
                Console.Write($"{item.Value.Name}, ");
                count++;
            }

        }
    }
}



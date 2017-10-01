using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppBZork.Entities
{
    class Item : GameObject
    {
        public string Details { get; set; }
        public bool CanBeTaken { get; set; }
        public string UseItemDescription { get; set; }
        public string Matches { get; set; }

        public string CombinedItemKey { get; set; }
        public Item CombinedItem { get; set; }
        public bool Persistent { get; set; }
        



        public Item(string name, string description, bool canBeTaken) : base(name, description)
        {
            CanBeTaken = canBeTaken;
        }

        public static void UseItemOnItem(string firstItem, string secondItem, Room currentRoom)
        {
            // Use item on item in room
            foreach (var target in currentRoom.listOfItems)
            {
                if (target.Key == secondItem) // Found target item
                {
                    foreach (var actor in Player.inventory)
                    {
                        if (actor.Key == firstItem) // Fount actor item
                        {
                            if (actor.Value.Matches == target.Key)
                            {
                                Console.WriteLine($"{target.Value.Description}");
                            }
                            else
                            {
                                Console.WriteLine("You can't use {0} on {1}", actor.Value.Name, target.Value.Name);
                            }
                            return;
                        }
                    }
                    Console.WriteLine($"You don't have {firstItem.ToLower()} in your inventory.");
                    return; // Found target but not actor
                }
            }
            // Open an Exit with an item
            foreach (var exit in currentRoom.listOfExits)
            {
                if (exit.Value.Name.ToUpper() == secondItem) // found target exit
                {
                    foreach (var actor in Player.inventory)
                    {
                        if (actor.Key == firstItem) // Fount actor item
                        {
                            if (actor.Value.Matches == exit.Value.Name.ToUpper())
                            {
                                if (exit.Value.Locked == false)
                                {
                                    Console.WriteLine($"{exit.Value.Name} is already open!");
                                }
                                else
                                {
                                    Console.WriteLine("You unlock {0}.", exit.Value.Name);
                                    exit.Value.Locked = false;
                                }

                            }
                            return;
                        }
                    }
                    Console.WriteLine($"You don't have {firstItem.ToLower()} in your inventory.");
                    return;
                }
            }
            // Combine two items in inventory
            foreach (var target in Player.inventory)
            {
                if (target.Key == secondItem) // Found target item
                {
                    foreach (var actor in Player.inventory)
                    {
                        if (actor.Key == firstItem) // Fount actor item
                        {
                            if (actor.Value.Matches == target.Key)
                            {
                                Player.inventory.Add(actor.Value.CombinedItemKey, actor.Value.CombinedItem);
                                if (actor.Value.Persistent == false) { Player.inventory.Remove(actor.Key); }
                                if (target.Value.Persistent == false) { Player.inventory.Remove(target.Key); }
                                Console.WriteLine($"{actor.Value.UseItemDescription}");
                            }
                            else
                                Console.WriteLine("You can't combine these items.");
                            return;
                        }
                    }
                    Console.WriteLine($"You don't have {firstItem.ToLower()} in your inventory.");
                    return;
                }
            }
            Console.WriteLine($"Can't find {secondItem.ToLower()} in the room or your inventory.");
        }
        public static void PickUpItem(string command, Room currentRoom)
        {
            foreach (var item in currentRoom.listOfItems) // Look in listOFItems for my Item
            {
                if (item.Key == command) // Found my item
                {
                    if (item.Value.CanBeTaken == true) // Can I Take item?
                    {
                        Console.WriteLine($"You picked up {item.Value.Name}!");
                        Player.inventory.Add(item.Key, item.Value);
                        currentRoom.listOfItems.Remove(item.Key);
                    }
                    else // No.
                    {
                        Console.WriteLine($"You cannot pick up {item.Value.Name}!");
                    }
                    return; // Already found the item so end no matter if I picked it up or not
                }
            }
            Console.WriteLine($"There is no such item in this room"); // I checked, promise.
        }

        public static void DropItem(string command, Room currentRoom)
        {
            foreach (var item in Player.inventory)
            {
                if (item.Key == command)
                {
                    Console.WriteLine($"You droped {item.Value.Name}!");
                    currentRoom.listOfItems.Add(item.Key, item.Value);
                    Player.inventory.Remove(item.Key);
                    return;
                }
            }
            Console.WriteLine($"There is no such item in your inventory");
        }
    }
}

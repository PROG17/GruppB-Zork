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
        public string UseItemActionDescription { get; set; }
        public string Match { get; set; }
        public string BadMatch { get; set; }
        public bool UseItemLooseGame { get; set; }

        public string CombinedItemKey { get; set; } 
        public Item CombinedItem { get; set; }
        public bool Persistent { get; set; }




        public Item(string name, string description, bool canBeTaken) : base(name, description)
        {
            CanBeTaken = canBeTaken;
        }

        public static void UseItemOnItem(string firstItem, string secondItem, Room currentRoom)
        {
            // Use item on item in room - does nothing!!
            foreach (var target in currentRoom.listOfItems)
            {
                if (target.Key == secondItem) // Found target item
                {
                    foreach (var actor in Player.inventory)
                    {
                        if (actor.Key == firstItem) // Found actor item
                        {
                            if ((actor.Value.UseItemLooseGame || target.Value.UseItemLooseGame) && actor.Value.BadMatch == target.Key)
                            {
                                if (target.Key == "COMPUTER")
                                {
                                Console.WriteLine("It was a trap the computer exploded in your face! GAME OVER "); Console.ReadLine(); 
                                Console.WriteLine("You died.");
                                }
                                else
                                {
                                    Console.WriteLine($"{target.Value.UseItemActionDescription}");
                                    Console.WriteLine("You died. \nPress any key to end...");
                                    Console.ReadKey();
                                }
                                Program.EndGame();
                                return;
                            }
                            if (actor.Value.Match == target.Key)
                            {
                                Player.inventory.Add(actor.Value.CombinedItemKey, actor.Value.CombinedItem);
                                if (actor.Value.Persistent == false) { Player.inventory.Remove(actor.Key); }
                                if (target.Value.Persistent == false) { currentRoom.listOfItems.Remove(target.Key); }
                                Console.WriteLine($"{actor.Value.UseItemActionDescription}");
                            }
                            else
                                Console.WriteLine("You can't use these items together.");
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
                            if (actor.Value.UseItemLooseGame && actor.Value.BadMatch == exit.Value.Name.ToUpper())
                            {
                                Console.WriteLine($"{actor.Value.UseItemActionDescription}");
                                Console.WriteLine("You died. \nPress any key to end...");
                                Console.ReadKey();
                                Program.EndGame();
                                return;
                            }
                            if (actor.Value.Match == exit.Value.Name.ToUpper())
                            {

                                if (exit.Value.Locked == false)
                                {
                                    Console.WriteLine($"{exit.Value.Name} is already open!");
                                }
                                else
                                {
                                    Console.WriteLine($"{actor.Value.UseItemActionDescription}");
                                    Console.WriteLine("You unlock {0}.", exit.Value.Name.ToLower());
                                    exit.Value.Locked = false;
                                }
                                return;
                            }
                            Console.WriteLine("You can't use {0} on {1}", firstItem.ToLower(), secondItem.ToLower());
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
                            if (((actor.Value.UseItemLooseGame || target.Value.UseItemLooseGame) && actor.Value.BadMatch == target.Key))
                            {
                                Console.WriteLine($"{actor.Value.UseItemActionDescription}");
                                Console.WriteLine("You died. \nPress any key to end...");
                                Console.ReadKey();
                                Program.EndGame();
                                return;
                            }
                            if (actor.Value.Match == target.Key)
                            {
                                Player.inventory.Add(actor.Value.CombinedItemKey, actor.Value.CombinedItem);
                                if (actor.Value.Persistent == false) { Player.inventory.Remove(actor.Key); }
                                if (target.Value.Persistent == false) { Player.inventory.Remove(target.Key); }
                                Console.WriteLine($"{actor.Value.UseItemActionDescription}");
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
                    Console.WriteLine($"You dropped {item.Value.Name}!");
                    currentRoom.listOfItems.Add(item.Key, item.Value);
                    Player.inventory.Remove(item.Key);
                    return;
                }
            }
            Console.WriteLine($"There is no such item in your inventory");
        }
        public static void Inspect(string command, Room currentRoom)
        {
            foreach (var item in currentRoom.listOfItems)
            {
                if (item.Key == command)
                {
                    Console.WriteLine(item.Value.Details);
                    return;
                }
            }
            foreach (var item in Player.inventory)
            {
                if (item.Key == command)
                {
                    Console.WriteLine(item.Value.Details);
                    return;
                }
            }
            foreach (var exit in currentRoom.listOfExits)
            {
                if (exit.Value.Name.ToUpper() == command)
                {
                    Console.WriteLine(exit.Value.Details);
                    return;
                }
            }
            Console.WriteLine($"There is no such item.");
        }
    }
}

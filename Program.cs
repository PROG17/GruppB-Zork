
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GruppBZork.Entities;


namespace GruppBZork
{
    class Program
    {
        Player player = new Player("Börje KrachDumi");
        static Room currentRoom;

        /// <summary>
        /// Exempel: > USE KEY ON LOCKEDDOOR. 
        /// Loopar igenom listan currentRoom.listOfItems om ett objekt matchar secondItem strängen ex. "LOCKEDDOOR".
        /// På samma sätt loopar man igenom inventory men för firstItem
        /// sen kollar man om firstItem objektet har secondItem obj i matches
        /// (actor.Value.Matches == target.Key) 
        /// Fungerar likadant för items on exits
        /// </summary>
        public static void UseItemOnItem(string firstItem, string secondItem)
        {
            foreach (var target in currentRoom.listOfItems) // Item on Item
            {
                if (target.Key == secondItem) // Found target item
                {
                    foreach (var actor in Player.inventory)
                    {
                        if (actor.Key == firstItem) // Fount actor item
                        {
                            if (actor.Value.Matches == target.Key)
                            {
                                // Return combined item method in item class
                                // Maybe remove ingoing items - Attribute of both items
                            }
                        }
                    }
                    return;
                }
            }
            foreach (var exit in currentRoom.listOfExits) // Item on Exit
            {
                if (exit.Key == secondItem) // found target exit
                {
                    foreach (var actor in Player.inventory)
                    {
                        if (actor.Key == firstItem) // Fount actor item
                        {
                            if (actor.Value.Matches == exit.Key)
                            {
                                exit.Value.Locked = false;
                                // Maybe remove ingoing items - Attribute of the actor item
                            }
                        }
                    }
                    return;
                }
            }
        }



        /// <summary>
        /// ShowInventory() Loopar igenom Player.inventory och skriver ut vad som finns där
        /// </summary>
        public static void ShowInventory()
        {
            Console.Write($"You have: ");
            bool noItemsInList = true;
            foreach (var item in Player.inventory)
            {
                if (Player.inventory.Count > 0)
                {
                    Console.Write($"{item.Value.Name} ");
                    noItemsInList = false;
                }
            }
            if (noItemsInList == true)
            { Console.WriteLine($"Zero items in your inventory!"); }
            else
            { Console.WriteLine($"in your inventory!"); }
        }

        /// <summary>PickUpItem(string userInput, Room currentRoom)       
        /// Metoden används för att plocka upp ett objekt. 
        /// Inparametrarna är här:
        /// <param name="userInput"></param>
        /// <param name="currentRoom"></param>
        /// Ifall objeket finns i rummet och "canBetakeable" så flyttas objeketet från 
        /// currentRoom.itemList ===> Player.inventory
        /// </summary>
        public static void PickUpItem(string userInput, Room currentRoom)
        {
            foreach (var item in currentRoom.listOfItems) // Look in listOFItems for my Item
            {
                if (item.Key == userInput) // Found my item
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

        /// <summary>DropItem(string userInput, Room currentRoom)       
        /// Metoden används för att droppa ett objekt. 
        /// Inparametrarna är här:
        /// <param name="userInput"></param>
        /// <param name="currentRoom"></param>
        /// Ifall objeket finns i Player.inventory så flyttas objeketet från 
        /// Player.inventory ===> currentRoom.itemList 
        /// </summary>
        public static void DropItem(string userInput, Room currentRoom)
        {
            foreach (var item in Player.inventory)
            {
                if (item.Value.Name == userInput.ToUpper())
                {
                    Console.WriteLine($"You droped {item.Value.Name}!");
                    currentRoom.listOfItems.Add(item.Key, item.Value);
                    Player.inventory.Remove(item.Key);
                    return;
                }
                else
                {
                    Console.WriteLine($"There is no such item in your inventory");
                }

            }
        }



        /* Ta vår input och dela upp den
         * ta första ordet och hitta rätt "metod"
         * räkna antal ord 
         * beroende på antal ord ska vi leta efter olika saker i listor.
         * returna ett/fler objekt.
         * 
         */

        static void GameEngine() // Input parser
        {
            string input = Console.ReadLine().ToUpper();
            string[] commandList = input.Split(' ');

            if (commandList[0] == "GO")
            {
                if (commandList[1] == "EAST" || commandList[1] == "WEST" || commandList[1] == "NORTH" || commandList[1] == "SOUTH")
                {
                    foreach (var exit in currentRoom.listOfExits)
                    {
                        if (commandList[1] == exit.Key)
                        {
                            currentRoom = currentRoom.listOfExits[exit.Key].Go(currentRoom);
                            currentRoom.DescribeRoom();
                            return;
                        }
                    }
                    Console.WriteLine("There is no exit in that direction.");
                    return;
                }
                else
                {
                    Console.WriteLine("Write a destination: East, West, North or South.\nEx. go east");
                    return;
                }
            }
            else if (commandList[0] == "TAKE")
            {
                foreach (var item in currentRoom.listOfItems)
                {
                    if (commandList[1] == item.Key)
                    {
                        PickUpItem(commandList[1], currentRoom);
                        return;
                    }
                }
                Console.WriteLine("There is no such item in this room.");
            }
            else if (commandList[0] == "DROP")
            {
                foreach (var item in Player.inventory)
                {
                    if (commandList[1] == item.Key)
                    {
                        DropItem(commandList[1], currentRoom);
                        return;
                    }
                }
                Console.WriteLine("You have no such item in your inventory.");
            }
            else if (commandList[0] == "USE")
            {
                if (commandList.Count() != 4)
                    Console.WriteLine("Write use <item> with <item> or use <item> with <exit>. \n Ex. use key on door, or use corkscrew on bottle.");
                else
                    Item.UseItemOnItem(commandList[1], commandList[3], currentRoom);
                return;
            }
            else
            {
                Console.WriteLine("Use the commands: Go, Use, Look or Inspect" );
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("ZORK: The Room Adventure \nCopyright (c) 2017 FAM Inc. All rights reserved");

            Console.WriteLine();


            Console.Write("Enter your name to start the game: ");
            string playerName = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine($"Welcome {playerName} to the Room Adventure!\n" +
                "In your hand there is a note that reads; \n\n\"We know that you're afraid, but there is no " +
                "time to doubt yourself because \nonly you can save yourself from this place. Defeat every " +
                "obstacle that comes your way and maybe, \njust maybe you'll get out of here alive... Good Luck!\"");
            Console.WriteLine("\n* press any key to continue *");
            Console.ReadKey();
        
            Console.WriteLine("\n\nHere is a list of commands you can use in the game: ");
            Console.WriteLine("-------------------------------------------------------------\n");

            Console.WriteLine("GO --> The directions in this game are: NORTH, SOUTH, EAST, WEST. \nEx. Type GO EAST to go in that direction.\n");
            Console.WriteLine("USE --> To use an item on another item, type USE \"ITEM\" ON \"ITEM\"\n");
            Console.WriteLine("TAKE --> Type TAKE followed by the name of the item to pick it up\n");
            Console.WriteLine("DROP --> Type DROP followed by the name of the item to drop an item\n");
            Console.WriteLine("LOOK --> Repeat description of the current room.\n");
            Console.WriteLine("INSPECT --> A more detailed description of an item\n");
            Console.WriteLine("SHOW INVENTORY --> Shows your inventory list\n");
            Console.WriteLine("HELP --> When you feel lost, type HELP and the Command List will pop up\n");

            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("* press any key to continue *");
            Console.ReadKey();
            Console.Clear();


            ///Ändrade namnen på testrummen och object för att lättare skilja på dem

            //Room 1
            Room firstRoom = new Room(name: "First Room", description: "Welcome to the first room");

            //Room 2
            Room secondRoom = new Room(name: "First Room", description: "Welcome to the second room");

            //Items
            firstRoom.listOfItems.Add("CORKSCREW", new Item(name: "Corkscrew", description: "This is a corkscrew", canBeTaken: true)
            {
                Matches = "BOTTLE",
                CombinedItemKey = "OPENED BOTTLE",
                CombinedItem = new Item ( name: "Opened bottle",
                description: "This is a an opened bottle",
                canBeTaken: true)

            });

            firstRoom.listOfItems.Add("BOTTLE", new Item(name: "Bottle", description: "This is a an unopened bottle", canBeTaken: true) { CombinedDescription ="Opened bottle", Matches = "CORKSCREW" });
            firstRoom.listOfItems.Add("KEY", new Item(name: "KEY", description: "This is a test KEY", canBeTaken: true) { Matches = "DOOR" });

            //Initialize Exits
            var door = new Exit(name: "DOOR", description: "Dörr mellan red och blue", locked: false, lockedDescription: "LOCKED!", room1: firstRoom, room2: secondRoom);
            
            //Exits add to lists of rooms
            firstRoom.listOfExits.Add("EAST", door);
            secondRoom.listOfExits.Add("WEST", door);

            //Start setup.
            currentRoom = firstRoom;
            currentRoom.DescribeRoom();

            while (true)
            {
                GameEngine();
            }
        }
    }
}


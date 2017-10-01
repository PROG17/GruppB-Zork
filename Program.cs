
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
            else if (commandList[0] == "SHOW" && commandList[1] == "INVENTORY")
            {
                Player.ShowInventory();
            }
            else
            {
                Console.WriteLine("Use the commands: Go, Use, Look or Inspect");
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
                Persistent = true,
                Matches = "BOTTLE",
                CombinedItemKey = "OPENED BOTTLE",
                CombinedItem = new Item(name: "Opened bottle",
                description: "This is a an opened bottle",
                canBeTaken: true)

            });

            firstRoom.listOfItems.Add("BOTTLE", new Item(name: "Bottle", description: "This is a an unopened bottle", canBeTaken: true)
            {
                Persistent = false,
                Matches = "CORKSCREW"
            });
            firstRoom.listOfItems.Add("KEY", new Item(name: "Key", description: "This is a test KEY", canBeTaken: true) { Matches = "DOOR" });

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


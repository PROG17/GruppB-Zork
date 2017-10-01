
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
        // "GLOBAL" < - wat
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
            Player player = new Player("Börje KrachDumi");

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


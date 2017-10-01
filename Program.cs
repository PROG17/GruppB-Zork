
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
        static Player player = new Player("Börje KrachDumi");
        static Room currentRoom;
        static bool GameOver = false;


        static void GameEngine() // Input parser
        {
            Console.Write("> ");
            string input = Console.ReadLine().ToUpper();
            string[] commandList = input.Split(' ');
            Console.WriteLine();
            if (commandList.Count() == 2)
            {
                if (commandList[0] == "GO")
                {
                    currentRoom = currentRoom.Go(commandList[1]);
                }
                else if (commandList[0] == "TAKE")
                {
                    Item.PickUpItem(commandList[1], currentRoom);
                }
                else if (commandList[0] == "DROP")
                {
                    Item.DropItem(commandList[1], currentRoom);
                }

                else if (commandList[0] == "SHOW" && commandList[1] == "INVENTORY")
                {
                    Player.ShowInventory();

                }
                else if (commandList[0] == "LOOK" && commandList[1] == "AROUND")
                {
                    currentRoom.DescribeRoom();
                }
                else
                {
                    Console.WriteLine("Write Help for info on how to use the commands.");
                }
            }
            else if (commandList.Count() == 4)
            {
                if (commandList[0] == "USE")
                    Item.UseItemOnItem(commandList[1], commandList[3], currentRoom);
                else
                    Console.WriteLine("Write Help for info on how to use the commands.");
            }
            else if (commandList[0] == "HELP")
            {
                Help();
            }
            else
            {
                Console.WriteLine("Write Help for info on how to use the commands.");
            }
            Console.WriteLine();

        }

        public static void Help()
        {
            Console.Clear();
            Console.WriteLine("\n\nHere is a list of commands you can use in the game: ");
            Console.WriteLine("-------------------------------------------------------------\n");

            Console.WriteLine("GO --> The directions in this game are: NORTH, SOUTH, EAST, WEST. \nEx. Type GO EAST to go in that direction.\n");
            Console.WriteLine("USE --> To use an item on another item, type USE \"ITEM\" ON \"ITEM\"\n");
            Console.WriteLine("TAKE --> Type TAKE followed by the name of the item to pick it up\n");
            Console.WriteLine("DROP --> Type DROP followed by the name of the item to drop an item\n");
            Console.WriteLine("LOOK AROUND --> Repeat description of the current room.\n");
            Console.WriteLine("INSPECT --> A more detailed description of an item\n");
            Console.WriteLine("SHOW INVENTORY --> Shows your inventory list\n");
            Console.WriteLine("HELP --> When you feel lost, type HELP and the Command List will pop up\n");

            Console.WriteLine("------------------------------------------------------------");


        }
        public static void StartupText()
        {
            Console.WriteLine("ZORK: The Room Adventure \nCopyright (c) 2017 FAM Inc. All rights reserved\n");
            Console.Write("Enter your name to start the game: ");
            player.Name = Console.ReadLine();

            Console.WriteLine($"\nWelcome {player.Name} to the Room Adventure!\n" +
                "In your hand there is a note that reads; \n\n\"We know that you're afraid, but there is no " +
                "time to doubt yourself because \nonly you can save yourself from this place. Defeat every " +
                "obstacle that comes your way and maybe, \njust maybe you'll get out of here alive... Good Luck!\"");
            Console.WriteLine("\n* press any key to continue *");
            Console.ReadKey();
        }

        public static void EndGame()
        {
            GameOver = true;
        }
        static void Main(string[] args)
        {


            //Room 1
            Room firstRoom = new Room(name: "First Room", description: "Welcome to the first room");

            //Room 2
            Room secondRoom = new Room(name: "First Room", description: "Welcome to the second room");

            Room thirdRoom = new Room(name: "Third Room", description: "Welcome to the third room");

            Room endRoom = new Room(name: "End Room", description: "Good Game!") { EndPoint = true };

            //Items
            firstRoom.listOfItems.Add("CORKSCREW", new Item(name: "Corkscrew", description: "This is a corkscrew", canBeTaken: true)
            {
                Persistent = true,
                Match = "BOTTLE",
                CombinedItemKey = "OPENED BOTTLE",
                CombinedItem = new Item(name: "Opened bottle",
                description: "This is a an opened bottle",
                canBeTaken: true),
                UseItemActionDescription = "You uncorked the bottle! ;)",



            });

            firstRoom.listOfItems.Add("BOTTLE", new Item(name: "Bottle", description: "This is a an unopened bottle", canBeTaken: true)
            {
                Persistent = false,
                Match = "CORKSCREW",
                CombinedItemKey = "OPENED",
                CombinedItem = new Item(name: "Opened bottle",
                description: "This is a an opened bottle",
                canBeTaken: true),
                UseItemActionDescription = "You uncorked the bottle! ;)",
            });
            firstRoom.listOfItems.Add("KEY", new Item(name: "Key", description: "This is a test KEY", canBeTaken: true) { BadMatch = "DOOR2" });

            //Initialize Exits
            var first_second = new Exit(name: "DOOR", description: "Dörr mellan red och blue", locked: false, lockedDescription: "LOCKED!", room1: firstRoom, room2: secondRoom);
            var second_end = new Exit(name: "DOOR2", description: "Dörr mellan blue och end", locked: false, lockedDescription: "LOCKED!", room1: endRoom, room2: secondRoom);
            
            //Exits add to lists of rooms
            firstRoom.listOfExits.Add("EAST", first_second);
            secondRoom.listOfExits.Add("WEST", first_second);
            secondRoom.listOfExits.Add("EAST", second_end);

            //Start setup.
            StartupText();
            Help();
            Console.WriteLine("* press any key to continue *");
            Console.ReadKey();

            currentRoom = firstRoom;
            currentRoom.DescribeRoom();

            while (GameOver == false)
            {
                GameEngine();
            }
        }
    }
}


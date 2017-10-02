
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
        static Player player = new Player();
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
                else if (commandList[0] == "INSPECT")
                {
                    Item.Inspect(commandList[1], currentRoom);
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
            //Start setup.
            StartupText();
            Help();
            Console.WriteLine("* press any key to continue *");
            Console.ReadKey();

            //Room 1
            Room firstRoom = new Room(name: "First Room", description: "Welcome to the first room");

            //Room 2
            Room secondRoom = new Room(name: "First Room", description: "Welcome to the second room");

            //Room 3 - Enter from North, dead end room to the south and end of the game to the east.
            Room thirdRoom = new Room(name: "Steamy Room", description: "The room is filled with steaming pipes, levers and valves." + 
                "\nThere's a sturdy looking vault door to the west, an open passage to the South and a door to the north.");

            Room fourthRoom = new Room(name: "Tiny storage", description: "It's a small storage area.");

            Room endRoom = new Room(name: "End Room", description: $"Freedom! \nYou live for now {player.Name}, good job!") { EndPoint = true };

            thirdRoom.listOfItems.Add("PIN", new Item("Pin", "A small metal rod.", true)
            {
                Details = "It's a small metal rod. There's some sign of wear as if its been used as part of a mechanism.",
                UseItemActionDescription = "You slot the pin into the round hole at the base of the lever and pull the lever." +
                "\nYou hear some whirring noises and then a slot opens in the wall next to the lever. \nInside you find a valve!",
                Persistent = false,
                Match = "LEVER",
                CombinedItemKey = "VALVE",
                CombinedItem = new Item(name: "Valve",
                description: "It's a large valve.",
                canBeTaken: true)
                { Details = "There's a hexagonal protrusion at the bottom of the valve, it looks like it fits onto something.",
                    UseItemActionDescription = "You slot the hexagonal protrusion of the valve into the slot on the vault door and turn it.",
                    BadMatch = "SAFE", Match = "VAULT" }
            });
            thirdRoom.listOfItems.Add("LEVER", new Item("Lever", "There's a large lever on the wall.", false)
            {
                Details = "There is a round hole at the base of the lever where a part seems to be missing.",
                UseItemActionDescription = "You slot the pin into the round hole at the base of the lever and pull the lever." +
                "\nYou hear some whirring noises and then a slot opens in the wall next to the lever. \nInside you find a valve!",
                Persistent = true,
                Match = "PIN",
                CombinedItemKey = "VALVE",
                CombinedItem = new Item(name: "Valve",
                description: "It's a large valve.",
                canBeTaken: true)
                { Details = "There's a hexagonal protrusion at the bottom of the valve, it looks like it fits onto something.", BadMatch = "SAFE", Match = "VAULT" }
            });

            fourthRoom.listOfItems.Add("SAFE", new Item("Safe", "Its a large metal safe. It could contain a lot of riches!", false)
            { UseItemActionDescription = "You attach the valve and turn it. When you open the safe you unleash a deadly gas!",
                Details = "On closer inspection you see a little skull and crossbones on the back of the safe.",
                UseItemLooseGame = true ,BadMatch = "VALVE"});
            
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
            var second_third = new Exit(name: "DOOR2", description: "Dörr mellan blue och end", locked: false, lockedDescription: "LOCKED!", room1: endRoom, room2: secondRoom);
            var third_end = new Exit(name: "VAULT", description: "A large Vault door.", locked: true, lockedDescription: "It won't even budge.", room1: endRoom, room2: thirdRoom) { Details = "It's a very sturdy metal door, there's a suspicious looking hexagonal slot on one side." };
            var third_fourth = new Exit(name: "CORRIDOR", description: "An open corridor.", locked: false, lockedDescription: "", room1: fourthRoom, room2: thirdRoom);
            
            //Exits add to lists of rooms
            firstRoom.listOfExits.Add("EAST", first_second);
            secondRoom.listOfExits.Add("WEST", first_second);
            secondRoom.listOfExits.Add("EAST", second_third);
            thirdRoom.listOfExits.Add("NORTH", second_third);
            thirdRoom.listOfExits.Add("WEST", third_end);
            thirdRoom.listOfExits.Add("SOUTH", third_fourth);
            fourthRoom.listOfExits.Add("NORTH", third_fourth);

            

            currentRoom = thirdRoom;
            currentRoom.DescribeRoom();

            while (GameOver == false)
            {
                GameEngine();
            }
        }
    }
}


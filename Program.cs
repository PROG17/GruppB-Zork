
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
                    Console.WriteLine("Write \"Help\" for info on how to use the commands.");
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
                "In your hand there's a note that reads; \n\n\"We know that you're afraid, but there is no " +
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
            Room firstRoom = new Room(name: "Blue Room", description: "WELCOME TO THE BLUE ROOM \nThe walls are painted in blue!");

            //Room 2
            Room secondRoom = new Room(name: "Laser Room", description: "WELCOME TO THE LASER ROOM \nThis room is completely black. Everything is dusty and the air reaks of rotten meat.. " +
                "\nCountless neon graffiti drawings cover all four walls and hundreds of discoballs hang from the ceiling. " +
                "\nThe room is also occupied by evil robot penguins who are ready to attack you. So BEWARE! ");

            //Room 3 - Enter from North, dead end room to the south and end of the game to the east.
            Room thirdRoom = new Room(name: "Steamy Room", description: "WELCOME TO THE STEAM ROOM \nThe room is filled with steaming pipes, levers and valves." + 
                "\nThere's a sturdy looking vault door to the west, an open passage to the South and a door to the north.");

            Room fourthRoom = new Room(name: "Tiny storage", description: "WELCOME TO THE STORAGE ROOM \nIt's a small storage area.");

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

            // Items
            firstRoom.listOfItems.Add("COMPUTER", new Item(name: "Computer", description: "Its a desktopcomputer, it has no power", canBeTaken: false)
            {
                Description = "Its a dell computer and the powercord is missing",
                Persistent = true,
                Match = "POWERCORD",
                CombinedItemKey = "WORKING_COMPUTER",
                CombinedItem = new Item(name: "COMPUTER2",
                description: "",
                canBeTaken: false),
                UseItemActionDescription = "",
                UseItemLooseGame=true,            

            });
            firstRoom.listOfItems.Add("POWERCORD", new Item(name: "Powercord", description: "Its a powercord", canBeTaken: true) {  Details = "Its a one meterlong powercord for a computer ", Match = "COMPUTER", BadMatch = "COMPUTER" });
            firstRoom.listOfItems.Add("SOFA", new Item(name: "Sofa", description: "Its a red threeseated sofa", canBeTaken: false) {  Details = "Its a red, three seated, worndown sofa. Lookes like it has seen its share of tinderdates"});
           

            firstRoom.listOfItems.Add("HAMMERHEAD", new Item(name: "Hammerhead", description: "Its the head of a hammer", canBeTaken: true) { Details = "Its a big ironhammerhead", Match = "STICK" });
            firstRoom.listOfItems.Add("STICK", new Item(name: "Stick", description: "Its a wooden stick", canBeTaken: true) {
                Details = "Its the head too an big ironhammer",
                Match = "HAMMERHEAD",
                CombinedItemKey = "HAMMER",
                CombinedItem = new Item(name: "Hammer",
                description: "Its an big awesome hammer",
                canBeTaken: true)
                {Match="PADLOCK", Persistent=true},
                UseItemActionDescription = "It was a perfect match, you now have an big, awesome *HAMMER* in your inventory",
                               
            });
            
            firstRoom.listOfItems.Add("KEY", new Item(name: "Key", description: "This is a test KEY", canBeTaken: true) { BadMatch = "DOOR2" });


            //Items secondRoom

            secondRoom.listOfItems.Add("LASERGUN", new Item(name: "Lasergun", description: "\nA powerful lasergun that will shoot your " +
                "opponent down in an instant. \nYou have limited amunition though so use it wisely.\n", canBeTaken: true)
            {
                Persistent = true,
                Match = "MAGASIN",
                CombinedItemKey = "LOAD",
                CombinedItem = new Item(name: "Loaded Lasergun",
                description: "This is a loaded Lasergun",
                canBeTaken: true),
                UseItemActionDescription = $"Whooaa, you just went hardcore Rambo and killed loads of the evil robot penguins in the room!! " +
                $"\nWe can't believe we underestimated you {player.Name}, you are truely awesome! \nBut wait...There's a strange noise coming from the cupboard.\n" +
                $"Oh no! There are still a few evil robot penguins alive. You need a quick solution to wipe them out",

            });

            secondRoom.listOfItems.Add("MAGASIN", new Item(name: "Magasin", description: "\nLoad your Lasergun with amunition. Trust me, you'll need it!\n", canBeTaken: true)
            {
                Persistent = true,
                Match = "LASERGUN",
                CombinedItemKey = "LOAD",
                CombinedItem = new Item(name: "Loaded Lasergun",
                description: "This is a loaded Lasergun",
                canBeTaken: true),
                UseItemActionDescription = $"Whooaa, you just went hardcore Rambo and killed loads of the evil robot penguins in the room!! " +
                $"\nWe can't believe we underestimated you {player.Name}, you are truely awesome! \nBut wait...There's a strange noise coming from the cupboard.\n" +
                $"Oh no! There are still a few evil robot penguins alive. You need a quick solution to wipe them out",

            });

            secondRoom.listOfItems.Add("BOMB", new Item(name: "Bomb", description: "\nThis Bomb will definitely wipe out everything living thing \nwithin a perimeter of 50 feet. " +
                "The bomb is poorly made with acid liquid running down from the sides. \nSo be very careful, skin contact may be fatal.\n", canBeTaken: true)
            {
                Persistent = false,
                Match = "CAN",
                CombinedItemKey = "THROW",
                CombinedItem = new Item(name: "A bomb contained in a can",
                description: "A bomb contained in a can",
                canBeTaken: true),
                UseItemActionDescription = $"\"B O O O M!!!\" \nYou threw the bomb and killed a bunch of evil robot penguins! Good job {player.Name}!",
            });

            secondRoom.listOfItems.Add("CAN", new Item(name: "Can", description: "\nAn empty can. Big enough to store items and light enough to throw.", canBeTaken: true)
            {
                Persistent = false,
                Match = "BOMB",
                CombinedItemKey = "THROW",
                CombinedItem = new Item(name: "A bomb contained in a can",
                description: "A bomb contained in a can",
                canBeTaken: true),
                UseItemActionDescription = $"\"B O O O M!!!\" \nYou threw the bomb and killed a bunch of evil robot penguins! Good job {player.Name}!",
            });



            //Initialize Exits
            var first_second = new Exit(name: "PADLOCK", description: "Its a stealdoor", locked: true, lockedDescription: "The door is locked with a padlock! ", room1: firstRoom, room2: secondRoom);
            var second_third = new Exit(name: "DOOR", description: "It's just a door.", locked: false, lockedDescription: "LOCKED!", room1: secondRoom, room2: thirdRoom);
            var third_end = new Exit(name: "VAULT", description: "A large Vault door.", locked: true, lockedDescription: "It won't even budge.", room1: endRoom, room2: thirdRoom) { Details = "It's a very sturdy metal door, there's a suspicious looking hexagonal slot on one side." };
            var third_fourth = new Exit(name: "CORRIDOR", description: "An open corridor.", locked: false, lockedDescription: "", room1: fourthRoom, room2: thirdRoom);
            
            //Exits add to lists of rooms
            firstRoom.listOfExits.Add("EAST", first_second);
            secondRoom.listOfExits.Add("WEST", first_second);
            secondRoom.listOfExits.Add("NORTH", second_third);
            thirdRoom.listOfExits.Add("SOUTH", second_third);
            thirdRoom.listOfExits.Add("WEST", third_end);
            thirdRoom.listOfExits.Add("NORTH", third_fourth);
            fourthRoom.listOfExits.Add("SOUTH", third_fourth);

            



            //Start setup.
            currentRoom = firstRoom;
            currentRoom.DescribeRoom();

            while (GameOver == false)
            {
                GameEngine();
            }
        }
    }
}


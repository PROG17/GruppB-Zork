﻿
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

        public static void Go(Exit destination)
        {
            foreach (var exit in currentRoom.listOfExits)
            {
                if (exit.Value == destination)
                {
                    currentRoom = exit.Value.GoThrough(currentRoom);
                }
            }
        }

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

        static void GameEngine()
        {
            string input = Console.ReadLine().ToUpper();
            string[] commandList = input.Split(' ');

            if (commandList[0] == "GO")
            {
                foreach (var exit in currentRoom.listOfExits)
                {
                    if (commandList[1] == exit.Key)
                    {
                        Go(exit.Value);
                        return;
                    }
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
            }
            else if (commandList[0] == "USE")
            {
                if (commandList.Count() == 4)
                {
                    UseItemOnItem(commandList[1], commandList[3]);
                }
                else if (commandList.Count() == 2)
                {
                    Console.WriteLine("Not implemented yet");
                    //UseItem(commandList[1]); 
                }
                return;
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
            Room blueRoom = new Room(name: "BLUEROOM", description: "Welcome to the blue room");

            //Room 2
            Room redRoom = new Room(name: "REDRROOM", description: "Welcome to the red room");

            //Items
            blueRoom.listOfItems.Add("KEY", new Item(name: "Key", description: "This is a test key", canBeTaken: true) { Matches = "LOCKEDDOOR" });
            blueRoom.listOfItems.Add("LOCKEDDOOR", new Item(name: "LockedDoor", description: "This is a test key", canBeTaken: true) { Matches = "KEY" });
            blueRoom.listOfItems.Add("SOFA", new Item(name: "Sofa", description: "This is a test sofa", canBeTaken: false));

            //Exits
            var redDoor = new Exit(name: "REDDOOR", description: "Door between red and blue", locked: true, room1: blueRoom, room2: redRoom);

            //ExitsAddToLists
            blueRoom.listOfExits.Add("EAST", redDoor);
            redRoom.listOfExits.Add("WEST", redDoor);

            currentRoom = blueRoom;

            while (true)
            {
                currentRoom.DescribeRoom();
                GameEngine();
            }
        }
    }
}


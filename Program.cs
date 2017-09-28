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
                if (exit == destination)
                {
                    currentRoom = exit.GoThrough(currentRoom);
                }
            }
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
            foreach (var item in currentRoom.listOfItems)
            {
                if (item.CanBeTaken == true && item.Name == userInput.ToUpper())
                {
                    Console.WriteLine($"You picked up {item.Name}!");
                    Player.inventory.Add(item);
                    currentRoom.listOfItems.Remove(item);
                    return;
                }
                else if (item.CanBeTaken == false && item.Name == userInput.ToUpper())
                {
                    Console.WriteLine($"You cannot pick up {item.Name}!");
                }
                else
                {
                    Console.WriteLine($"There is no {userInput} in this room");
                }
            }
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
                if (item.Name == userInput.ToUpper())
                {
                    Console.WriteLine($"You droped {item.Name}!");
                    currentRoom.listOfItems.Add(item);
                    Player.inventory.Remove(item);
                    return;
                }
                else
                {
                    Console.WriteLine($"There is no {userInput} in your inventory");
                }

            }
        }

        static void Main(string[] args)
        {
            ///Ändrade namnen på testrummen och object för att lättare skilja på dem

            //Room 1
            Room blueRoom = new Room(name: "BLUEROOM", description: "Welcome to the blue room");

            //Room 2
            Room redRoom = new Room(name: "REDRROOM", description: "Welcome to the red room");

            //Items
            blueRoom.listOfItems.Add(new Item(name: "KEY", description: "This is a test key", canBeTaken: true));
            blueRoom.listOfItems.Add(new Item(name: "SOFA", description: "This is a test sofa", canBeTaken: false));

            //Exits
            blueRoom.listOfExits.Add(new Exit(name: "REDDOOR", description: "This is a test door", locked: true) { room1 = blueRoom, room2 = redRoom });
            redRoom.listOfExits.Add(new Exit(name: "BLUEDOOR", description: "This is a test door 2", locked: false) { room1 = blueRoom, room2 = redRoom });



            

            while (true)
            {
                currentRoom = blueRoom;
                currentRoom.DescribeRoom();

                string input = Console.ReadLine().ToUpper();

                if (input == "GO")
                {
                    Go(currentRoom.listOfExits[0]);
                }
                if (input == "PICK KEY" || input == "PICK SOFA")///Testar PickUpItem och dropItem
                {
                    if (input == "PICK KEY")
                    { input = "KEY"; }
                    else { input = "SOFA"; }

                    foreach (var item in currentRoom.listOfItems)
                    {
                        if (item.Name == input)
                        { PickUpItem(input, currentRoom); break; }
                    }
                }
                if (input == "DROP KEY")    ///Testar DropIte´m
                {
                  input = "KEY"; 
                    
                    foreach (var item in Player.inventory)
                    {
                        if (item.Name == "KEY")
                        { DropItem(input, currentRoom); break; }
                    }
                }
            }
        }
    }
}


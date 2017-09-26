
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
        static Dictionary<RN, Room> rooms = new Dictionary<RN, Room>();
        static Dictionary<IN, Thing> inventory = new Dictionary<IN, Thing>();

        static void SetupGame()
        {
            // Rooms set
            Room room1 = new Room("Main Room", "This is the main room.");
            rooms.Add(RN.Main, room1);
            rooms.Add(RN.Living, new Room("Living Room", "This is the Living room."));
            
            rooms.Add(RN.Second, new Room("Second Room", "This is a Second Room. The door locked behind you."));
            rooms.Add(RN.Cellar, new Room("Cellar", "This is the cellar."));

            // Exits setup
            rooms[RN.Main].exits.Add(Direction.East, rooms[RN.Living]);
            rooms[RN.Main].exits.Add(Direction.West, rooms[RN.Second]);
            rooms[RN.Living].exits.Add(Direction.West, rooms[RN.Main]);
            
            // Contents setup
            inventory.Add(IN.Poison, new Item("Poison", "This is poison, it will kill you."));
            rooms[RN.Main].contents.Add(IN.DoorKey, new Item("Door key", "key to the hall."));
            // Direct Cast
            ((Item)inventory[IN.Poison]).Details = "The label says it will errode metal";
        }

        static void Main(string[] args)
        {
            SetupGame();
            Room currentRoom = rooms[RN.Main];

            while (true)
            {
                currentRoom.Describe();
                Console.Write(">");
                // Read commands from user
                string command = Console.ReadLine().ToUpper();
                string[] commandList = command.Split(' ');
                if (commandList[0] == "GO") // Ex. "go east"
                {
                    currentRoom = Go(commandList[1], currentRoom);
                }
                else if (commandList[0] == "LOOK" || commandList[0] == "CHECK") // LookAt
                {
                    if (commandList[1] == "AROUND" || commandList[1] == "AT" && commandList[2] == "ROOM")
                        LookAt("ROOM", currentRoom);
                    else if (commandList[1] == "AT")
                    {
                        LookAt(commandList[2], currentRoom);
                    }
                    else
                    {
                        LookAt(commandList[1], currentRoom);
                    }
                        
                }
                else if (command == "QUIT")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("I don't understand.");
                    Console.WriteLine("Try: " + string.Join(", ", Enum.GetNames(typeof(Actions))) + "\n");
                }
            }
        }

        static void LookAt(string command, Room currentRoom)
        {
            if (command == "ROOM")
            {
                currentRoom.Describe();
            }
            if (Enum.TryParse(command, true, out IN itemName))
            {
                if (inventory.TryGetValue(itemName, out Thing item))
                {
                    Console.WriteLine("You look at your: " + item.Name);
                    if (item is Item)
                    {
                        if (((Item)item).Details == null)
                            Console.WriteLine("Its not very interesting.");
                        else
                            Console.WriteLine(((Item)item).Details);
                    }
                }
                else if (currentRoom.contents.TryGetValue(itemName, out item))
                {
                    Console.WriteLine("You look at the: " + item.Name);
                    if (item is Item)
                    {
                        if (((Item)item).Details == null)
                            Console.WriteLine("Its not very interesting.");
                        else
                            Console.WriteLine(((Item)item).Details);
                    }
                }
            }
            else
            {
                Console.WriteLine("Theres nothing like that to look at");
            }
        }

        static Room Go(string command, Room currentRoom)
        {
            if (Enum.TryParse(command, true, out Direction direction)) // Tries to parse input as one of the Enums (case insensitive)
            {
                if (currentRoom.exits.TryGetValue(direction, out Room destination))
                {
                    currentRoom = destination;
                }
                else
                {
                    Console.WriteLine("You can't go that way.");
                }
            }
            else
            {
                Console.WriteLine("I don't understand.");
                Console.WriteLine("Try: " + string.Join(", ", Enum.GetNames(typeof(Direction))) + "\n");
            }
            return currentRoom;
        }

    }
}

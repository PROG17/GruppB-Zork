﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppBZork.Entities
{

    class Room : GameObject

    {
        public Dictionary<string, Exit> listOfExits = new Dictionary<string, Exit>();
        public Dictionary<string, Item> listOfItems = new Dictionary<string, Item>();
        public bool EndPoint { get; set; }

        public Room(string name, string description) : base(name, description)
        {
            Name = name;
            Description = description;
        }

        public void DescribeRoom()
        {
            if (EndPoint)
            {
                Console.WriteLine(Description);
                Program.EndGame();
                return;
            }
            Console.Clear();
            Console.WriteLine(Description);
            Console.WriteLine();
            Console.Write("Here is a list of items in the room: \n\n");
            foreach (var item in listOfItems)
            {
                Console.Write($"{item.Value.Name}: {item.Value.Description}\n");
            }
            Console.WriteLine("\nRoom exits: ");
            foreach (var exit in listOfExits)
            {
                Console.WriteLine($"To the {exit.Key}: {exit.Value.Description}");

            }
            Console.WriteLine();
        }

        public Room Go(string direction)
        {
            if (direction == "EAST" || direction == "WEST" || direction == "NORTH" || direction == "SOUTH")
            {
                foreach (var exit in listOfExits)
                {
                    if (direction == exit.Key)
                    {
                        Room newRoom = exit.Value.GoThrough(this);
                        if (newRoom == this)
                        {
                            return newRoom;
                        }
                        else
                        {
                            newRoom.DescribeRoom();
                            return newRoom;
                        }
                    }
                }
                Console.WriteLine("There is no exit in that direction.");
                return this;
            }
            Console.WriteLine("Write directions: east, west, north or south.");
            return this;
        }
    }
}

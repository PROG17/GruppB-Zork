
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
        static Room currentRoom;
        public static void Go(Exit destination)
        {

            foreach (var item in currentRoom.listOfExits)
            {
                Console.WriteLine(item);

                if (item == destination)
                {

                    //this is a comment
                }
            }
            //currentRoom = resultRoom; 

        }

        static void Main(string[] args)
        {

            Room testRoom = new Room(name: "Test Room", description: "Welcome to Test Room");
            Room testRoom2 = new Room(name: "Test Room Two", description: "Welcome to Test Room Two");

            testRoom.listOfItems.Add(new Item(name: "Test Key", description: "This is a test key"));
            testRoom.listOfExits.Add(new Exit(name: "Test Door", description: "This is a test door"));
            testRoom.listOfExits.Add(new Exit(name: "Test Door 2", description: "This is a test door 2"));

            currentRoom = testRoom;

            while (true)
            {
                currentRoom.DescribeRoom();

                string input = Console.ReadLine().ToUpper();

                if (input == "GO")
                {
                    Go(currentRoom.listOfExits[0]);
                    Console.WriteLine("go method");
                }
            }
        }
    }
}


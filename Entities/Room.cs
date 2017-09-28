using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppBZork.Entities
{

    class Room : GameObject

    {
        public List<Exit> listOfExits = new List<Exit>();

        public List<Item> listOfItems = new List<Item>();

        public Room(string name, string description) : base(name, description)
        {
            Name = name;
            Description = description;
        }

        public void DescribeRoom()
        {
            Console.WriteLine(Description);
            Console.WriteLine();
            Console.Write("Here is a list of items in the room: ");
            Console.WriteLine();
            foreach (var item in listOfItems)
            {
                Console.Write($"{item.Name}: {item.Description} ");
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("Room exits: ");
            foreach (var exit in listOfExits)
            {
                Console.WriteLine($"{exit.Name}: {exit.Description}");

            }
        }
    }
}

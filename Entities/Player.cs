using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppBZork.Entities
{
    class Player
    {
        public string Name { get; set; }
        public static List<Item> inventory = new List<Item>();

        public Player(string name)
        {
            Name = name;
            //Name = Console.ReadLine();            
        }
    }
}



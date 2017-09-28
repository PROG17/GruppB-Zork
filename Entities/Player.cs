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

        List<Item> inventory = new List<Item>();

    }

}

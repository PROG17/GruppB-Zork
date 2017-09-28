﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppBZork.Entities
{
   class Exit : GameObject
    {
       public Room room1;
       public Room room2;

        public Exit(string name, string description) : base(name, description)
        {
            Name = name;
            Description = description;
        }


        public Room GoThrough(Room currentRoom)
        {
            if (room1 == currentRoom)
            {
                return room2;
            }
            else
            {
                return room1;
            }
        }
    }
}


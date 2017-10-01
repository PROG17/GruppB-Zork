using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppBZork.Entities
{
    class Exit : GameObject
    {
        public Room Room1 { get; set; }
        public Room Room2 { get; set; }
        public bool Locked;
        public string LockedDescription { get; set; }

        public Exit(string name, string description, bool locked, string lockedDescription, Room room1, Room room2) : base(name, description)
        {
            Room1 = room1;
            Room2 = room2;
            Locked = locked;
            LockedDescription = lockedDescription;
        }

        public Room GoThrough(Room currentRoom)
        {
            if (Locked)
            {
                Console.WriteLine(LockedDescription);
                return currentRoom;
            }
            if (Room1 == currentRoom)
            {
                return Room2;
            }
            else
            {
                return Room1;
            }
        }
    }
}


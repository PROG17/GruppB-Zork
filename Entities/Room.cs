using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppBZork.Entities
{
    class Room : Container
    {
        public Dictionary<Direction, Room> exits = new Dictionary<Direction, Room>();

        public Room(string Name, string Description) : base(Name, Description)
        {
            
        }

        public void Describe()
        {
            string exitsText = string.Join(", ", exits.Keys.ToArray());
            string contentsText = string.Join(", ", contents.Keys.ToArray());
            if (string.IsNullOrEmpty(exitsText))
            {
                exitsText = "None";
            }
            Console.WriteLine("{0}\n\n{1}\n\n{2}\n\nExits Are: {3}\n", Name, Description, contentsText, exitsText);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppBZork.Entities
{
    class Item : GameObject
    {
        public string Details { get; set; }
        public bool CanBeTaken { get; set; }

        /// <summary>
        /// Om man behöver para ihop två object så finns några extra parametrar.
        /// Tanken är att man ska skriva in deras namn och lika så på det andra
        /// itemet så blir de ett par.
        /// </summary>
        public string pairUpWith1 { get; set; }
        public string pairUpWith2 { get; set; }
        public string pairUpWith3 { get; set; }

        public Item(string name, string description, bool canBeTaken) : base(name, description)
        {
            CanBeTaken = canBeTaken;
        }
    }
}

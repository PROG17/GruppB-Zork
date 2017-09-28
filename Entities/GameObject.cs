using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppBZork.Entities
{
    abstract class GameObject
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public GameObject(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}

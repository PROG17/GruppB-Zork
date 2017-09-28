using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppBZork.Entities
{
    class Exit : GameObject
    {
        public Exit(string name, string description) : base(name, description)
        {
            Name = name;
            Description = description;
        }
    }
}


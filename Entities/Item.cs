﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppBZork.Entities
{
    class Item : GameObject
    {
        public string Details { get; set; }
        public Item(string name, string description) : base(name, description)
        {
        }
    }
}

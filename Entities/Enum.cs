﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GruppBZork.Entities
{
    enum Direction
    {
        North,
        South,
        East,
        West,
    }

    // Room Names
    enum RN
    {
        Main,
        Living,
        Second,
        Cellar
    }

    // Item Names
    enum IN
    {
        Poison,
        DoorKey,
        LaserGun,
        BodyArmour,
        EvilRobotPeng,
        SmokeBomb,
        Cage,
        DiscoBall

    }

    enum Actions
    {
        Go,
        Use,
        Look,

    }
}

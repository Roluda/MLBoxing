using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MLBoxing.Ragdoll {
    [Flags]
    public enum LimbType {
        Head = 1<<0,
        Torso = 1<<1,
        Hips = 1<<2,
        LeftUpperArm = 1<<3,
        LeftLowerArm = 1<<4,
        LeftPalm = 1<<5,
        RightUpperArm = 1<<6,
        RightLowerArm = 1<<7,
        RightPalm = 1<<8,
        LeftThigh = 1<<9,
        LeftShin = 1<<10,
        LeftFoot = 1<<11,
        RightThigh = 1<<12,
        RightShin = 1<<13,
        RightFoot = 1<<14
    }
}

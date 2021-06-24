using System;

namespace MLBoxing.Ragdoll {
    [Flags]
    public enum JointType {
        Neck = 1 << 0,
        Chest = 1 << 1,
        LeftShoulder = 1 << 2,
        LeftElbow = 1 << 3,
        LeftHand = 1 << 4,
        RightShoulder = 1 << 5,
        RightElbow = 1 << 6,
        RightHand = 1 << 7,
        LeftHip = 1 << 8,
        LeftKnee = 1 << 9,
        LeftVerse = 1 << 10,
        RightHip = 1 << 11,
        RightKnee = 1 << 12,
        RightVerse = 1 << 13
    }
}

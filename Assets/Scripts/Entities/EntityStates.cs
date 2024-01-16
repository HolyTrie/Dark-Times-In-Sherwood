using System;
using UnityEngine;
// https://stackoverflow.com/questions/19582477/combining-enum-values-with-bit-flags
// https://stackoverflow.com/questions/1030090/how-do-you-pass-multiple-enum-values-in-c
// https://stackoverflow.com/questions/8447/what-does-the-flags-enum-attribute-mean-in-c

static class EntityStates
{
    [Flags] 
    public enum AllStates
    {
        IDLE   =  0,
        WALK   =  1 << 0,
        RUN    =  1 << 1,
        JUMP   =  1 << 3,
        FALL   =  1 << 4,
        DEATH  =  1 << 5,
        ATTACK  =  1 << 6,
        TOOKHIT =  1 << 7
    }
}

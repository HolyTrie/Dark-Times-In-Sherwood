using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

namespace DTIS{
    public class ShroomAI : EntityBrain
    {
        int _counter = 0;
        int _sum = 0;
        readonly int len = 75;
        protected override void Logic()
        {
            FSM.SetState(ESP.States.Grounded,ESP.States.Walk);
            _counter = (_counter + 1) % len;
            ++_sum;
            //Debug.Log("Ticks = " + _sum + " ||| Counter = " + _counter);
            if(_counter == 0)
            {
                FSM.Direction = -1*FSM.Direction;
            }
        }
    }
}
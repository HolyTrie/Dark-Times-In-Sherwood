using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace DTIS{
    public class ShroomAI : EntityBrain
    {
        int _counter = 0;
        int _sum = 0;
        readonly int len = 50;
        protected override void Update()
        {
            return;
        }
        protected override void FixedUpdate()
        {   
            _counter = (_counter + 1) % len;
            ++_sum;
            Debug.Log("Ticks = " + _sum + " ||| Counter = " + _counter);
            if(_counter == 0)
            {
                Direction *= -1;
                if(Direction == 1)
                {   
                    FSM.SetState(ESP.States.Grounded,ESP.States.Walk);
                }
                else
                {
                    FSM.SetState(ESP.States.Grounded,ESP.States.Idle);
                }
            }
        }
    }
}
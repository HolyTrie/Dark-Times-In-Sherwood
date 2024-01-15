using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTIS{
    public class ShroomBehaviour : IEntityBehaviour
    {
        int dir = 1;
        int count = 0;
        int len = 5;
        public void Idle()
        {
            count += dir;
            if(Mathf.Abs(count) >= len){
                dir *= -1;
            }
        }
        public void Update(IEntityMovement movement, EntityStateMachine fsm)
        {   
            return;

        }
        public void FixedUpdate(IEntityMovement movement, EntityStateMachine fsm, EntityController con)
        {   
            //if state == something --> act accordingly
            Idle();
            movement.Walk(con);

        }
    }
}
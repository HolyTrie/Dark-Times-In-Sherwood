using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTIS{
    public class ShroomBehaviour : EntityBehaviour
    {
        float dir = 1f;
        float count = 0f;
        float len = 10f;

        //public void onPlayerSighted();
        //public void onLostSightOfPlayer();
        //public void Attack();
        //public void Idle();
        public void Patrol()
        {
            count += 0.1f * dir;
            if(Mathf.Abs(count) >= len){
                dir *= -1;
            }
        }
        public override void Update()
        {
            return;
        }
        public override void FixedUpdate()
        {   
            //if state == something --> act accordingly
            Patrol();
            m_controller.Move(dir);
        }
    }
}
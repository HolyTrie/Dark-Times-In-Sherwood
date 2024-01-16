using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTIS{
    public class ShroomBehaviour : EntityBehaviour
    {
        int dir = 1;
        int count = 0;
        int len = 5;

        //public void onPlayerSighted();
        //public void onLostSightOfPlayer();
        //public void Attack();
        //public void Idle();
        public void Idle()
        {
            count += dir;
            if(Mathf.Abs(count) >= len){
                dir *= -1;
            }
        }
        public void FixedUpdate()
        {   
            //if state == something --> act accordingly
            Idle();
        }
    }
}
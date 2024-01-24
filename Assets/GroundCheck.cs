using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
        private bool _grounded = true;
        public bool Grounded{get{return _grounded;} set{_grounded = value;}}
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Other:" + _grounded);
            if(other.tag == "Floor")
                _grounded = true;
        }
        private void OnTriggerExit2D()
        {
            _grounded = false;
        }
}

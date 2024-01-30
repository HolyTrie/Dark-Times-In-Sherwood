using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTIS
{
    /*This class checks if the player is standing on ground or not*/
    public class GroundCheck : MonoBehaviour
    {
        private bool _grounded = true;
        public bool Grounded { get { return _grounded; } set { _grounded = value; } }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Floor") || other.CompareTag("Platform"))
                _grounded = true;
        }
        private void OnTriggerExit2D()
        {
            _grounded = false;
        }
    }
}

using UnityEngine;

namespace DTIS
{    public class ShroomMovement : MonoBehaviour, IEntityMovement 
    {
        public void Walk(EntityController con)
        {
            Vector3 targetVelocity = new Vector2(con.WalkSpeed(), con.velY()); // Move the character by finding the target velocity
            con.applySmoothDamp(targetVelocity); // And then smoothing it out and applying it to the character
        }
        public void Jump(EntityController con)
        {
            return;
        }

        public void Run(EntityController con)
        {
            return;
        }
    }
}
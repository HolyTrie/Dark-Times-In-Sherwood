using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * This component spawns the given object whenever the player clicks a given key.
 */
namespace DTIS
{
    public class ClickSpawn : MonoBehaviour
    {
        [SerializeField] protected GameObject prefabToSpawn;
        [SerializeField] protected Vector3 velocityOfSpawnedObject;
        [SerializeField] float Delay;
        [SerializeField] Transform spawnPosition;


        // private PlayerStateMachine fsm;
        // private bool canSpawn = true;
        public void spawnObject() // is it ok to keep as public?
        {
            Debug.Log("Spawning a new object");

            // Step 1: spawn the new object.
            Vector3 positionOfSpawnedObject = spawnPosition.position;  // span at the containing object position.
            Quaternion rotationOfSpawnedObject = Quaternion.identity;  // no rotation.
            // GameObject newObject = 
            Instantiate(prefabToSpawn, positionOfSpawnedObject, rotationOfSpawnedObject);
            MoveObject newObjectMover = prefabToSpawn.GetComponent<MoveObject>();
            if (newObjectMover) {
                newObjectMover.SetVelocity(velocityOfSpawnedObject);
            }
            // Step 2: modify the velocity of the new object.
            // return newObject;
        }

        // private void Update()
        // {
            
        //     if (fsm.Controls.ActionMap.Shoot.WasPressedThisFrame() && canSpawn)
        //     {
        //         this.StartCoroutine(DelayLaser());
        //     }
        // }

        //delays the user from shooting every "Delay" seconds.
    //     private IEnumerator DelayLaser()
    //     {
    //         Debug.Log("Cannon is loading...");

    //         canSpawn = false;
    //         spawnObject();

    //         yield return new WaitForSeconds(Delay);

    //         canSpawn = true;
    //     }
    }
}
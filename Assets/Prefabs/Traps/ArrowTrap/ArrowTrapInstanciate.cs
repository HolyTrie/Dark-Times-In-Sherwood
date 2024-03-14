using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTIS;
public class ArrowTrapInstanciate : MonoBehaviour
{
    [SerializeField] protected GameObject prefabToSpawn;
    [SerializeField] Transform spawnPosition;
    [SerializeField] Transform launchPoint;
    [SerializeField] private float _timeToLiveSeconds = 5f;

    /* This method generates an object according to the mouse cursor position */
    public void spawnObject()
    {
        //Set positions//
        Vector3 rotation = transform.position - launchPoint.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        launchPoint.rotation = Quaternion.Euler(0, 0, rotZ);
        var go = Instantiate(prefabToSpawn, spawnPosition.position, Quaternion.identity);
        StartCoroutine(Util.DestroyGameObjectCountdown(go, _timeToLiveSeconds));
    }
}

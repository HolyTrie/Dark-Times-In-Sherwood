using System.Collections;
using System.Collections.Generic;
using DTIS;
using UnityEngine;

public class RopeOnClick : MonoBehaviour
{
    private bool _isPlayerAttached = false;
    private PlayerStateMachine _fsm = null;
    void Start()
    {
        //do we need something here?
    }
    void Update()
    {
        //listen to player input
        if(_isPlayerAttached)
        {
            _fsm.SetState(ESP.States.Climbing,ESP.States.Idle);
        }
    }

    private void onPlayerClick(GameObject player){
        player.transform.parent = transform;
        player.transform.position = transform.position;
        _isPlayerAttached = true;
        _fsm = player.GetComponent<PlayerStateMachine>();
    }
    private void onPlayerCancel()
    {
        _isPlayerAttached = false;
    }
}

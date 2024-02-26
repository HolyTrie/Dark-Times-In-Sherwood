using DTIS;
using UnityEngine;

public class TakedownInteractable : Interactable, IStealthable
{
    [SerializeField] private GameObject _goToTakedown;
    /*[SerializeField]*/ private bool _KillsEntity = false;
    [Tooltip("How long it take to take down an entity")]
    [SerializeField] private float _timeToTakedownSeconds = 0.025f;
    private GameObject _takeDownOrigin;
    public GameObject GO => _goToTakedown;

    public override void OnClick(GameObject clickingEntity)
    {
        if (clickingEntity.CompareTag("Player"))
        {
            _takeDownOrigin = clickingEntity;
            ((IStealthable)this).Takedown(_KillsEntity);
        }
    }

    void IStealthable.Takedown(bool kill)
    {
        if (kill)
        {
            throw new System.NotImplementedException();
        }
        else
        {
            var playerController = _takeDownOrigin.GetComponent<PlayerController>();
            var currPos = playerController.transform.position;
            var targetPos = GO.transform.position;
            var distance = Vector2.Distance(currPos,targetPos);
            var hit = Physics2D.Raycast(currPos,targetPos,distance,playerController.WhatIsGround);
            if(hit)
                return;
            var newPos = Vector2.Lerp(currPos, targetPos, 0.5f); //TODO; shunpo(teleport) instead of LERP!
            playerController.transform.position = new(newPos.x,newPos.y,playerController.transform.position.z);
            StartCoroutine(Util.DestroyGameObjectCountdown(_goToTakedown, _timeToTakedownSeconds));
        }
    }

    public float FoV => throw new System.NotImplementedException();

    public float Awareness => throw new System.NotImplementedException();
}
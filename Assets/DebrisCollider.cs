using DTIS;
using UnityEngine;
public class DebrisCollider : MonoBehaviour
{
    [SerializeField] private PlayerController playerController; //TODO: generalize for EntityController when it's time to.
    private bool _facingRight;
    private Vector3 _fixedPos;
    void Start()
    {
        _facingRight = playerController.FacingRight;
        _fixedPos = transform.localPosition;
    }
    void Update()
    {
       FollowEntityPosition();
       FollowEntityFlip();
    }
    private void FollowEntityPosition()
    {
       transform.position = playerController.transform.position + _fixedPos;
    }
    private void FollowEntityFlip()
    {
        if(_facingRight != playerController.FacingRight) //flip this object when the player flips.
        {
            _facingRight = !_facingRight;
            _fixedPos.x *= -1;
        }
    }
}

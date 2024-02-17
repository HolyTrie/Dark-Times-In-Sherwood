using System.Linq.Expressions;
using DTIS;
using UnityEngine;
public class DebrisCollider : MonoBehaviour
{
    [SerializeField] private PlayerController _controller; //TODO: generalize for EntityController when it's time to.
    private bool _facingRight;
    private Vector3 _fixedPos;

    private void Awake()
    {
        if (_controller == null)
        {
            try
            {
                _controller = transform.parent.GetComponentInChildren<PlayerController>();
            }
            catch (System.Exception)
            {
                throw; // parent must include player controller as a child.
            }
        }
    }

    void Start()
    {
        _facingRight = _controller.FacingRight;
        _fixedPos = transform.localPosition;
    }
    void Update()
    {
        Util.MimicEntityMovement(transform, _controller.transform, _fixedPos);
        FollowEntityFlip();
    }
    private void FollowEntityFlip()
    {
        if (_facingRight != _controller.FacingRight) //flip this object when the player flips.
        {
            _facingRight = !_facingRight;
            _fixedPos.x *= -1;
        }
    }
}

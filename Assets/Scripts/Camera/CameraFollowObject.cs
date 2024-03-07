using System.Collections;
using System.Collections.Generic;
using DTIS;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [Header("Refreneces")]
    [SerializeField] private Transform _playerTransform;

    [Header("Flip Rotation Stats")]
    [SerializeField] private float _flipYRotationTime = 0.5f;

    private Coroutine _flipCoroutine;
    private PlayerController _player;
    private bool _isFacingRight;

    private void Awake()
    {
        _player = _playerTransform.gameObject.GetComponent<PlayerController>();
        _isFacingRight = _player.FacingRight;
    }

    void Update()
    {
        //make the CamerafollowObject follow the player's position
        transform.position = _playerTransform.position;
    }

    public void Flip()
    {
        _flipCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while(elapsedTime <_flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;

            //lerp y rotation
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime/_flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f,yRotation,0f);

            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        _isFacingRight = !_isFacingRight;

        if(_isFacingRight)
        {
            return 180f;
        }
        else
        {
            return 0f;
        }
    }
}

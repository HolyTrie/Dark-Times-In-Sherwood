using System.Collections;
using System.Collections.Generic;
using DTIS;
using UnityEngine;

public class SawTrapController : MonoBehaviour
{
    [SerializeField] private Transform _activeSprite;
    private Animator _animatorSaw;

    // Start is called before the first frame update
    void Start()
    {
        _animatorSaw= _activeSprite.gameObject.GetComponent<Animator>(); // at first i dont want the animation to start.
        _animatorSaw.SetBool("Attack",false);
    }

    public void StartSaw()
    {
        _animatorSaw.SetBool("Attack",true); // when he press the pressure plate, start playing the animation
    }

    // private IEnumerator StartSaw()
}

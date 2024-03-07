using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{
    public static ParticleSystemManager instance;

    [Header("Player Particles")]
    [SerializeField] public ParticleSystem JumpEffect;
    [SerializeField] public ParticleSystem DashEffect;
    [SerializeField] public Transform JumpTransform;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void playJumpEffect()
    {
        Instantiate(JumpEffect, new(JumpTransform.position.x, JumpTransform.position.y, -15f), JumpTransform.rotation);
        Debug.Log("ParticleSystems");
        // JumpEffect.Play();
    }

    public void playDashEffect()
    {
        Instantiate(DashEffect, new(JumpTransform.position.x, JumpTransform.position.y, -10f), JumpTransform.rotation);
    }



}

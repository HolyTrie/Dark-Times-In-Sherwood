using UnityEngine;

public interface IStealthable
{
    public GameObject GO{get;}
    public float FoV{get;}
    public float Awareness{get;}
    public void Takedown(bool kill = false);
}
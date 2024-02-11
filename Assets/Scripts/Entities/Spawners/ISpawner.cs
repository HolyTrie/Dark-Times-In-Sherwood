using UnityEngine;

public interface ISpawner
{
    public GameObject Prefab{get;}
    public void TrySpawn();
}
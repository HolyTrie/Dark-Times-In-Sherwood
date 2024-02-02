using UnityEngine;

public interface IClimbable
{
    void Attach(GameObject entity);
    void Deattach();
    bool HasAttachedEntity{get;}
}

public abstract class Climbable : MonoBehaviour, IClimbable
{
    private GameObject _attachedEntity; //currently only one entity can be attached.
    public GameObject AttachedEntity{get => _attachedEntity; private set =>_attachedEntity = value;}
    public bool HasAttachedEntity { get => _attachedEntity!=null; }
    public void Attach(GameObject entity)
    {
        AttachedEntity = entity;
        OnAttach();
    }
    public void Deattach()
    {
       OnDeattach();
    }
    protected abstract void OnAttach();
    protected abstract void OnDeattach();
}
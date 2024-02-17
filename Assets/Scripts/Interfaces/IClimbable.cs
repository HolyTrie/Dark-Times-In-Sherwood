using UnityEngine;

public interface IClimbable
{
    void Attach(GameObject entity, GameObject RopeSegment);
    void Deattach();
    bool HasAttachedEntity { get; }
}

public abstract class Climbable : MonoBehaviour, IClimbable
{
    private GameObject _attachedEntity; //currently only one entity can be attached.
    public GameObject AttachedEntity { get => _attachedEntity; private set => _attachedEntity = value; }
    public bool HasAttachedEntity { get => _attachedEntity != null; }
    public void Attach(GameObject entity, GameObject RopeSegment)
    {
        AttachedEntity = entity;
        OnAttach(RopeSegment);
    }
    public void Deattach()
    {
        // AttachedEntity = null; NO!
        OnDeattach();
    }
    protected abstract void OnAttach(GameObject RopeSegment);
    protected abstract void OnDeattach();
}
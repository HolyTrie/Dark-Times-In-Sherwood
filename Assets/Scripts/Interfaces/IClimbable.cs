using UnityEngine;

public interface IClimbable
{
    void Attach(Transform entity);
    void OnAttach();
    void OnDeattach();
}
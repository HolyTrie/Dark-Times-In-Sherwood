using UnityEngine;

public interface IInteractable // this allows me to use C# interfaces to reference Interactable
{
    void SetGUI(bool value);
    void OnClick(GameObject entity);
}

public abstract class Interactable : MonoBehaviour, IInteractable // while this allows me to use getComponent<Interactable>()
{
    [SerializeField] private GameObject _gui;
    protected void Start()
    {
        _gui.SetActive(false);
    }
    public virtual void SetGUI(bool value)
    {
        _gui.SetActive(value);
    }
    public abstract void OnClick(GameObject entity);
}
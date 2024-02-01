using UnityEngine;

public interface IInteractable
{
    void SetGUI(bool value);
    void OnClick();
}

public abstract class Interactable : MonoBehaviour, IInteractable //this allows me to use getComponent<Interactable>()
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
    public virtual void OnClick()
    {
        throw new System.NotImplementedException();
    }
}
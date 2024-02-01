using UnityEngine;

public class RopeController : MonoBehaviour,IClimbable
{
    // Start is called before the first frame update
    [SerializeField] private RopeGenerator _generatorScript;
    void Start()
    {
        if(_generatorScript == null)
        {
            _generatorScript = GetComponent<RopeGenerator>();
        }
    }

    public void Attach(Transform entity)
    {
        throw new System.NotImplementedException();
    }

    public void OnAttach()
    {
        throw new System.NotImplementedException();
    }

    public void OnDeattach()
    {
        throw new System.NotImplementedException();
    }
}

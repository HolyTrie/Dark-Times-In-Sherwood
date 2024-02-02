using UnityEngine;

public class RopeController : Climbable
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
    protected override void OnAttach()
    {
        Debug.Log("On Attach");
    }
    protected override void OnDeattach()
    {
        Debug.Log("On Deattach");
    }
}

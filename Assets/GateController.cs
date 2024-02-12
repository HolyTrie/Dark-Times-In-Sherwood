using System;
using System.Collections;
using DTIS;
using UnityEngine;

public class GateController : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer[] _SpriteChilds; 

    void Awake()
    {
        int i = 0;
        _SpriteChilds = new SpriteRenderer[transform.childCount];

        for(i=0;i<_SpriteChilds.Length;++i)
		{
			_SpriteChilds[i] = transform.GetChild(i).GetComponent<SpriteRenderer>(); 
        }

    }

    public void OpenGate()
    {
        for(int i = _SpriteChilds.Length - 1 ; i >= 0 ; --i)
        {
            StartCoroutine(WaitForSprite(i));
            
        }
    }

    private IEnumerator WaitForSprite(int idx)
    {
        yield return new WaitForSeconds(0.5f);
        _SpriteChilds[idx].enabled = false;
    }
}

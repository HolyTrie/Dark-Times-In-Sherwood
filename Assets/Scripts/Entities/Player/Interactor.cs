using System.Collections.Generic;
using DTIS;
using UnityEngine;
public class Interactor : MonoBehaviour
{
    //public Transform Referencepoint;
    private readonly IDictionary<int,Transform> _transformsInRange = new Dictionary<int,Transform>();
    [SerializeField] private PlayerController controller;
    private Transform closestObject;
    private Vector3 _fixedPos;
    void Start()
    {
        _fixedPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        SetClosestObject();
        foreach(var transform in Util.NearestNTransforms(_transformsInRange,controller.transform.position,10))
            Debug.Log(transform.gameObject.name);
        FollowEntityPosition();
    }
    private void SetClosestObject()
    {
        if(_transformsInRange.Count == 0)
            closestObject = null;
        else
            closestObject = Util.NearestNTransforms(_transformsInRange,controller.transform.position)[0]; //return array with one object
    }
    private void FollowEntityPosition()
    {
       transform.position = controller.transform.position + _fixedPos;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.CompareTag("Interactable"))
        {
            if(!_transformsInRange.ContainsKey(other.gameObject.GetHashCode()))
            {  
                _transformsInRange.Add(other.gameObject.GetHashCode(),other.transform);
                // NOTE - USING GetHashCode() AS A UNIQUE KEY IS BAD PRACTICE OUTSIDE OF UNITY!
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Interactable"))
        {
            _transformsInRange.Remove(other.gameObject.GetHashCode());
        }
    }
}
   
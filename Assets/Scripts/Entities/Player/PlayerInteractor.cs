using System.Collections.Generic;
using DTIS;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerInteractor : MonoBehaviour
{
    //TODO: swap to raycast interactor
    //[Tooltip("rays will be drawn in a circle around the playerController")]
    //[SerializeField] private float _numberOfRays;
    //private 
    private readonly Dictionary<int, Transform> _objectsInRange = new();
    private PlayerController _controller;
    public PlayerController Controller { get { return _controller; } internal set { _controller = value; } }
    private GameObject closestObject;
    public void Interact()
    {
        //Debug.Log("Attempting to Interact");
        if (closestObject != null)
        {
            closestObject.GetComponent<Interactable>().OnClick(Controller.gameObject);
        }
    }
    void Start()
    {
        _controller = Util.GetPlayerController();
    }

    // Update is called once per frame
    void Update()
    {
        SetClosestObject();
    }
    private void SetClosestObject()
    {
        if (closestObject != null)
            closestObject.GetComponent<Interactable>().SetGUI(false);
        if (_objectsInRange.Count == 0)
        {
            closestObject = null;
        }
        else
        {
            closestObject = Util.NearestNTransforms(_objectsInRange, transform.position)[0].gameObject; //returns array with one object.
            if (closestObject != null)
                closestObject.GetComponent<Interactable>().SetGUI(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            if (!_objectsInRange.ContainsKey(other.gameObject.GetHashCode()))
            {
                _objectsInRange.Add(other.gameObject.GetHashCode(), other.transform);
                // NOTE - USING GetHashCode() AS A UNIQUE KEY IS BAD PRACTICE OUTSIDE OF UNITY!
                // it just happens to be unique in this engine.
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            _objectsInRange.Remove(other.gameObject.GetHashCode());
        }
    }
}

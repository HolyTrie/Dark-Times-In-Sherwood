using UnityEngine;

public class Oscillator : MonoBehaviour
{
    const float tau = Mathf.PI * 2;
    [SerializeField]
    float maxAngle = 70f;
    [SerializeField]
    float speed = 1f;

    float omega;
    float amplitude;
    float _x, _y, _z;

    void Start() // Start is called before the first frame update
    {
        Debug.Log("Start");
        omega = speed;
        amplitude = maxAngle;
    }

    // Update is called once per frame
    void Update()
    {
        /*  
            we really struggled with this one, finally we found this solution which uses unity Quaternions
            instead of localRotation or RigidBody.
            Users 'Awesome2819' comment in https://discussions.unity.com/t/problem-with-oscillation/23029/3
        */
        _x = transform.localRotation.x;
        _y = transform.localRotation.y;
        _z = amplitude * Mathf.Sin(Time.time * omega);
        UnityEngine.Quaternion _rotation = UnityEngine.Quaternion.Euler(_x, _y, _z);
        transform.localRotation = _rotation;
    }

}

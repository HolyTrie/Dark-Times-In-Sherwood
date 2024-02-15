using UnityEngine;
using UnityEngine.Rendering;

public class ParallaxChildren : MonoBehaviour
{
	/// <summary>
	///  This script does exactly as the name suggests, this allows us to stack componenets into one layer and parallax those layers,
	///  instead of every sprite/image individually
	///  Built by referencing these sources:
	///  1. https://www.youtube.com/watch?v=5E5_Fquw7BM&ab_channel=Brackeys
	///  2. https://www.youtube.com/watch?v=zit45k6CUMk&ab_channel=Dani
	/// </summary>
	[SerializeField] private Transform _cam;
	[SerializeField] public float smoothing = 1f; // How smooth the parallax is going to be.  Make sure to set this above 0 for desired effect.
	[SerializeField] private SpriteRenderer lengthRef;
	//private float[] _startPos;

	private Vector3 previousCamPos; // the position of the camera in the previous frame
	private float[] _parallaxScales;
	private float _lengthRef;
	private float _baseParallax;
	private float _layers;

	void Awake()
	{
		// set up the camera reference
		_cam = Camera.main.transform;
		_lengthRef = lengthRef.bounds.size.x;
		_parallaxScales = new float[transform.childCount];
		if (smoothing <= 0)
		{
			smoothing = 1f; // just a randomly selected default
		}
	}
	void Start()
	{
		int max = 0;
		int i;
		for (i = 0; i < _parallaxScales.Length; ++i)
		{
			_parallaxScales[i] = transform.GetChild(i).position.z * -1; // brackeys trick. (refer to update to see how it's used)
			if (max < transform.GetChild(i).GetComponent<SortingGroup>().sortingOrder)
				max = transform.GetChild(i).GetComponent<SortingGroup>().sortingOrder;
		}
		//_layers = max;
		_baseParallax = 1f / max;
		//Debug.Log(_baseParallax);
	}

	void Update()
	{
		/// <summary>
		/// This is Brackeys approach to Parallax with small changes.
		/// </summary>
		Transform curr;
		float parallax;
		Vector3 newChildPos;
		for (int i = 0; i < transform.childCount; i++)
		{
			curr = transform.GetChild(i);
			parallax = (previousCamPos.x - _cam.position.x) * _parallaxScales[i]; //brackeys trick - the parallax moves opposite of the camera because the previous frame is multiplied by the scale
			newChildPos = new(curr.position.x + parallax, curr.position.y, curr.position.z); // new position for current child - only x is updated by the parallax value
			curr.position = Vector3.Lerp(curr.position, newChildPos, smoothing * Time.deltaTime); // interpolate where the camera should be next
		}
		previousCamPos = _cam.position; // Critical!
	}

	/*
	void Update () //todo optimize for fixed update
	{
		// Dani's approach - leaving this here as reference,
		// doesnt pre compute the parallax.
		for(int i = 0; i < transform.childCount; ++i)
		{
			int count = transform.GetChild(i).GetComponent<SortingGroup>().sortingOrder - 1;
			float parallexEffect = count * _baseParallax;
			Debug.Log(string.Format("parallax = {0} | _length = {1} | count = {2}",parallexEffect,_lengthRef,count));
			float temp = _cam.transform.position.x * (1-parallexEffect);
			float dist = _cam.transform.position.x*parallexEffect;

			transform.position = new Vector3(_startPos[i] + dist, transform.position.y, transform.position.z);

			if      (temp > _startPos[i] + _lengthRef) {_startPos[i] += _lengthRef;}
			else if (temp < _startPos[i] - _lengthRef) {_startPos[i] -= _lengthRef;}
		}
		
	}
	*/

}

using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

public class ParallaxChildren : MonoBehaviour {

	//private float length, startPos;
	[SerializeField] private GameObject _cam;
	[SerializeField] private SpriteRenderer lengthRef;
	private float[] _startPos;
	private float _lengthRef;
	private float _baseParallax;
	void Start () 
	{
		_lengthRef = lengthRef.bounds.size.x;
		int len = transform.childCount;
		int max = 0;
		_startPos = new float[len];
		int i;
		for(i=0;i<len;++i)
		{
			_startPos[i] = transform.position.x;
			if(max < transform.GetChild(i).GetComponent<SortingGroup>().sortingOrder)
				max = transform.GetChild(i).GetComponent<SortingGroup>().sortingOrder;
		}
		_baseParallax = 1f / max; 
		Debug.Log(_baseParallax);
	}
	
	void FixedUpdate () 
	{
		for(int i = 0; i < transform.childCount; ++i)
		{
			int count = transform.GetChild(i).GetComponent<SortingGroup>().sortingOrder - 1;
			float parallexEffect = 1f - _baseParallax * count;
			Debug.Log(string.Format("parallax = {0} | _length = {1} | count = {2}",parallexEffect,_lengthRef,count));
			float temp = _cam.transform.position.x * (1-parallexEffect);
			float dist = _cam.transform.position.x*parallexEffect;

			transform.position = new Vector3(_startPos[i] + dist, transform.position.y, transform.position.z);

			if      (temp > _startPos[i] + _lengthRef) {_startPos[i] += _lengthRef;}
			else if (temp < _startPos[i] - _lengthRef) {_startPos[i] -= _lengthRef;}
		}
		
	}

}

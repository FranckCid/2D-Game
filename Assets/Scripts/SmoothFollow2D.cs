using UnityEngine;
using System.Collections;

public class SmoothFollow2D : MonoBehaviour {
	
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	public Vector3 cameraDifference;

	void Start(){
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	// Update is called once per frame
	void Update () 
	{
		if (target)
		{	
			Vector3 _aux = target.position + cameraDifference;
			Vector3 point = Camera.main.WorldToViewportPoint(_aux);
			Vector3 delta = _aux - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
		
	}
}
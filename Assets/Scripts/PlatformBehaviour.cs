using UnityEngine;
using System.Collections;

public class PlatformBehaviour : MonoBehaviour {

	[SerializeField]
	private Transform player;
	private bool isUp;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (player.transform.position.y > transform.position.y) {
			this.GetComponent<BoxCollider2D>().enabled = true;
		} else {
			this.GetComponent<BoxCollider2D>().enabled = false;
		}
	}
}

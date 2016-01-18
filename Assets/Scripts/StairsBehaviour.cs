using UnityEngine;
using System.Collections;

public class StairsBehaviour : MonoBehaviour {

	[SerializeField]
	private Transform player;
	[SerializeField]
	private Transform top;
	[SerializeField]
	private Transform bottom;

	private GameObject colliding;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
			if (player.GetComponent<PlayerBehaviour> ().colliding && (player.transform.position.y > top.position.y || player.transform.position.y < bottom.position.y)) {
				colliding = player.GetComponent<PlayerBehaviour> ().colliding;
				if ((colliding.name == "Top" || colliding.name == "Bottom") &&
					Input.GetKeyDown (KeyCode.DownArrow)) {
					player.GetComponent<Rigidbody2D> ().gravityScale = 0;
					player.GetComponent<PlayerBehaviour> ().SetActualMechanic (Mechanic.CLIMBING);
				}
			}

			if (player.GetComponent<PlayerBehaviour> ().GetActualMechanic () == Mechanic.CLIMBING) {
				if (Input.GetKeyDown (KeyCode.UpArrow)) {
					player.GetComponent<PlayerBehaviour> ().SetActualMechanic (Mechanic.NORMAL);
				}
				if (Input.GetKey (KeyCode.S)) {
					Vector3 e = bottom.position - player.position;
					player.transform.Translate (e * Time.deltaTime);
					Debug.Log (e.y >= 1);
				} else if (Input.GetKey (KeyCode.W)) {
					Vector3 e = top.position - player.position;
					player.transform.Translate (e * Time.deltaTime);
					Debug.Log (e.y <= 1);
				}
			}
	}

}

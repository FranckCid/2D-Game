using UnityEngine;
using System.Collections;

public class ArrowBehaviour : MonoBehaviour {
	
	private Transform player;
	[SerializeField]
	private Transform bow;

	private bool alive;

	private Rigidbody2D rig;
	private Collider2D thisCol;

	void Start(){
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		rig = GetComponent<Rigidbody2D> ();
		alive = true;
		thisCol = GetComponent<Collider2D> ();
		this.transform.Rotate(new Vector3(0, 0, 360.0f - Vector3.Angle(this.transform.right, Vector2.left * 1)));
	}
	
	void FixedUpdate(){
		if (alive) {
			if (player.transform.position.x < this.transform.position.x) {
				this.transform.Rotate (new Vector3 (0, 0, Time.deltaTime * 100));
			} else if(player.transform.position.x > this.transform.position.x){
				this.transform.Rotate (new Vector3 (0, 0, Time.deltaTime * -100));
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if (!col.gameObject.tag.Equals ("Enemy")) {
			if(col.gameObject.tag.Equals("Player")){
				col.gameObject.GetComponent<PlayerBehaviour>().Damage(5);
				Destroy(this.gameObject);
			}else{
				Destroy (thisCol);
				Destroy(rig);
			}
			alive = false;
		}
	}

}

using UnityEngine;
using System.Collections;

public class RangedArcher : EnemyBehaviour {

	[SerializeField]
	private Transform bow;

	[SerializeField]
	private GameObject arrow;
	
	override protected void Move(){
		counter += Time.deltaTime;

		Vector3 dif = this.transform.position - player.transform.position;

		if(dif.y < range*6 && dif.x < range*6 && dif.y > -range*6 && dif.x > -range*6){

			Vector3 bowPos = bow.position - new Vector3(0.3f, 0, 0);
			Quaternion bowRot = Quaternion.Euler(new Vector3(0, 0, player.transform.position.x + player.transform.position.y *  10));

			if (player.transform.position.x < this.transform.position.x) {
				Quaternion rot = Quaternion.Euler (new Vector3 (0, 0, 0));
				this.transform.rotation = rot;
			} else if(player.transform.position.x > this.transform.position.x){
				Quaternion rot = Quaternion.Euler(new Vector3(0, 180, 0));
				bowPos = bow.position + new Vector3(0.3f, 0, 0);
				bowRot = Quaternion.Euler(new Vector3(0, 180, player.transform.position.x + player.transform.position.y *  10));
				this.transform.rotation = rot;
			}

			Debug.Log("Locked");
			if(counter >= 2){
				GameObject g = Instantiate(arrow, bowPos, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
				Vector2 fv = new Vector2 ((player.transform.position.x - bow.position.x) * 1.5f, player.transform.position.y - bow.position.y);
				g.GetComponent<Rigidbody2D>().AddForce(fv, ForceMode2D.Impulse);
				counter = 0;
			}

			bow.rotation = bowRot;

		}

	}

	override protected void Away(){

	}

}

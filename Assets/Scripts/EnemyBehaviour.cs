using UnityEngine;
using System.Collections;

public class EnemyBehaviour : CharacterBase {

	[SerializeField]
	protected GameObject player;
	[SerializeField]
	protected Animator anim;
	protected bool candoanything;
	protected Rigidbody2D rig;
	protected bool isAway;

	void Awake(){
		rig = GetComponent<Rigidbody2D> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		candoanything = true;
	}


	void Update () {
		if(candoanything){
			Move ();
		}
	}

	protected float counter = 0;
	protected float walkCount = 1;
	protected virtual void Move(){
		walkCount += Time.deltaTime;
		
		Vector3 dif = this.transform.position - player.transform.position;
		
		if (player.transform.position.x < this.transform.position.x && !isAway) {
			Quaternion rot = Quaternion.Euler (new Vector3 (0, 0, 0));
			this.transform.rotation = rot;
		} else if(player.transform.position.x > this.transform.position.x && !isAway){
			Quaternion rot = Quaternion.Euler(new Vector3(0, 180, 0));
			this.transform.rotation = rot;
		}
		if(dif.y < range*3 && dif.x < range*3 && dif.y > -range*3 && dif.x > -range*3){
			
			isAway = false;
			float multipliyer = 1.2f;
			if (dif.y < range*multipliyer && dif.x < range*multipliyer && dif.y > -range*multipliyer && dif.x > -range*multipliyer) {
				counter += Time.deltaTime;
				if (counter > 2f) {
					anim.SetBool ("isAttacking", true);
					counter = 0;
					this.Attack ();
				} else if (counter > 1.5f) {
					anim.SetBool ("isAttacking", false);
				}
			}else{
				if(walkCount > 1.5f){
					if (player.transform.position.x < this.transform.position.x && !isAway) {
						Vector2 v = new Vector2(2.5f * range*2, 0);
						this.rig.velocity = -v;
					} else if(player.transform.position.x > this.transform.position.x && !isAway){
						Vector2 v = new Vector2(2.5f * range*2, 0);
						this.rig.velocity = v;
					}
					walkCount = 0;
				}
				counter = 0;
			}
		} else if(dif.y < range*4 && dif.x < range*4 && dif.y > -range*4 && dif.x > -range*4){
			this.Away();
		}
	}

	protected override void Attack(){
		anim.SetBool ("isAttacking", true);

		Vector3 dir;

		if (transform.rotation.y >= 1) {
			dir = new Vector3 (range, 0, 0);
		} else {
			dir = new Vector3 (-range, 0, 0);
		}

		RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, dir, this.range) ;
		
		foreach(RaycastHit2D h in hit){
			if(h.collider != null){
				if(h.collider.tag.Equals("Player")){
					Debug.Log(this.name + "has hited " + h.collider.name + " with " + strenght + "hit point.");
					h.collider.gameObject.GetComponent<PlayerBehaviour>().Damage(strenght);
					h.collider.gameObject.GetComponent<PlayerBehaviour>().Knockback(0);
				}
			}
		}
	}
	
	public override void Damage (int d)
	{
		if (this.life <= 0) {
			Destroy(this.gameObject);
		}
		this.life -= d;
	}

	protected float thinktime = 5;
	protected virtual void Away(){
		isAway = true;
		thinktime += Time.deltaTime;
		if (thinktime > 0.5f && player.transform.position.x - this.transform.position.x < range*3) {
			if (Random.value > .5) {
				Vector2 v = new Vector2(0.5f * Random.value, 0);
				this.rig.velocity = v;
				Quaternion rot = Quaternion.Euler(new Vector3(0, 180, 0));
				this.transform.rotation = rot;
			}
			else {
				Vector2 v = new Vector2(0.5f * Random.value, 0);
				this.rig.velocity = -v;
				Quaternion rot = Quaternion.Euler(new Vector3(0, 0, 0));
				this.transform.rotation = rot;
			}
			thinktime = 0;
		}
	}

	public override void Knockback(float q){
		if (q <= 0)
			this.rig.AddForce (Vector3.left*2.5f, ForceMode2D.Impulse);
		else
			this.rig.AddForce (Vector3.left*q, ForceMode2D.Impulse);
		candoanything = false;
		StartCoroutine(Blink());
	}

	IEnumerator Blink() {
		yield return new WaitForSeconds(.3f);
		candoanything = true;
	}

}

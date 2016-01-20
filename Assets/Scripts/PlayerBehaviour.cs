using UnityEngine;
using System.Collections;

public enum Mechanic{
	NORMAL,
	CLIMBING
}

public class PlayerBehaviour : CharacterBase {


	[Header ("only a test, search about header and region/endregion")]
	[SerializeField]
	private float velocity;
	[SerializeField]
	private Transform graphics;
	[SerializeField]
	private Animator anim;
	private Rigidbody2D rig;
	private bool canWalk;
	private string actualWall;
	private bool canScalling;
	public GameObject colliding;
	private bool isDefending;
	public bool canDoAnything;
	private bool ignoreColls;
	private bool canMove;
	private Collider2D thisCol;

	private Mechanic actualMechanic;

	// Use this for initialization
	protected void Start () {
		canDoAnything = true;
		actualMechanic = Mechanic.NORMAL;
		rig = this.GetComponent<Rigidbody2D>();
		canWalk = true;
		canMove = true;
		canScalling = true;
		thisCol = GetComponent<Collider2D> ();
	}
	
	// Update is called once per frame
	float backtodo = 0;
	void FixedUpdate () {
		if (canDoAnything && canMove) {
			CheckMove ();
			CheckActions ();
		} else {
			backtodo += Time.deltaTime;
			if(backtodo >= 1.5f){
				canMove = true;
				canDoAnything = true;
				foreach(GameObject g in GameObject.FindGameObjectsWithTag("Enemy")){
					Physics2D.IgnoreCollision(thisCol, g.GetComponent<Collider2D>(), true);
				}
				StartCoroutine(Blink());
			}
		}
		if(backtodo > 1.0f)
			backtodo += Time.deltaTime;
		if(backtodo >= 5.0f){
			ignoreColls = false;
			backtodo = 0;
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("Enemy")){
				Physics2D.IgnoreCollision(thisCol, g.GetComponent<Collider2D>(), false);
			}
		}
	}
	
	//---------------------------------------------
	float counter = 2;

	void CheckActions(){
		counter += Time.deltaTime ;
		if (actualMechanic == Mechanic.NORMAL) {
			anim.SetBool ("isDefending", false);
			if (Input.GetKey (KeyCode.LeftArrow)) {
				anim.SetBool ("isDefending", true);
				isDefending = true;
			} else if (Input.GetAxis ("Jump") > 0 && anim.GetBool ("isGrounded") && !anim.GetBool ("isCrouching")) {
				rig.AddForce (new Vector2 (0, 80.5f) * Time.deltaTime, ForceMode2D.Impulse);
				anim.SetBool ("isJumping", true);
			} else if (Input.GetAxis("Attack") != 0/* && anim.GetBool("isAttacking") == false*/) {
				if(counter > 0.4f){
					counter = 0;
					this.Attack();
				}
			} else if (Input.GetAxis("Attack") == 0) {
				canWalk = true;
				anim.SetBool ("isAttacking", false);
			} else if (Input.GetKey (KeyCode.S)) {
				anim.SetBool ("isCrouching", true);
				canWalk = false;
			} else if (!Input.GetKey (KeyCode.S)) {
				anim.SetBool ("isCrouching", false);
				canWalk = true;
			} else if (Input.GetKeyDown (KeyCode.DownArrow) && canScalling) {
				if(actualWall != null){
					if(actualWall == "WallScalable"){
//						actualMechanic = Mechanic.CLIMBING_Y;
						rig.gravityScale = 0;
					}
				}
			}
		}
	}

	void CheckMove(){
		if (actualMechanic == Mechanic.NORMAL) {
			if (Input.GetAxisRaw("Horizontal") < 0) {
				Quaternion pos  = Quaternion.Euler(Vector3.zero);
				graphics.rotation = pos;
			} else if (Input.GetAxisRaw("Horizontal") > 0) {
				Quaternion pos = Quaternion.Euler(new Vector3(0, 180, 0));
				graphics.rotation = pos;
			}

			anim.SetBool ("isWalking", false);
			if ((Input.GetAxisRaw("Horizontal") != 0) && anim.GetBool ("isDefending") == false && canWalk) {
				rig.velocity = new Vector2 (Input.GetAxis("Horizontal") * velocity * Time.deltaTime, rig.velocity.y);
				//if(!anim.GetBool("isJumping"))
				anim.SetBool ("isWalking", true);
			}

		}

	}

	protected override void Attack(){
		anim.SetBool ("isAttacking", true);
		if (!anim.GetBool ("isJumping"))
			canWalk = false;

		Vector3 here = transform.position + new Vector3(0, 0.3f, 0);
		Vector2 dir;

		if (graphics.rotation.y >= 1) {
			dir = new Vector3 (range, 0, 0);
		} else {
			dir = new Vector3 (-range, 0, 0);
		}

		RaycastHit2D[] hit = Physics2D.RaycastAll(here, dir, this.range) ;

		foreach(RaycastHit2D h in hit){
			if(h.collider != null){
				//Debug.Log("Raycast hitted to: " + h.collider.name);
				if(h.collider.tag.Equals("Enemy")){
					Debug.Log(this.name + "has hited " + h.collider.name + " with " + this.strenght + "hit point.");
					h.collider.gameObject.GetComponent<CharacterBase>().Damage(this.strenght);
					h.collider.gameObject.GetComponent<CharacterBase>().Knockback(0);
				}
			}
		}
	}
	
	public override void Damage (int d)
	{
		if (!ignoreColls) {
			this.life -= d;
			if (life <= 0) {
				anim.SetBool ("isDead", true);
			}
		}
	}

	public override void Knockback(float q){
		int to = 1;
		if (graphics.transform.rotation.y > 0)
			to *= -1;

		if (canDoAnything  && !ignoreColls) {
			if(q == 0){
				this.rig.velocity = new Vector3 (to*70f, 70f*2, 0) * Time.deltaTime;
			}else{
				this.rig.velocity = new Vector3 (to*q, q*2, 0) * Time.deltaTime;
			}
			Damage (5);
			ignoreColls = true;
			canMove = false;
			canDoAnything = false;
		}
	}

	IEnumerator Blink() {
		if(ignoreColls){
			graphics.GetComponent<SpriteRenderer>().enabled = false;
			yield return new WaitForSeconds(.2f);
			graphics.GetComponent<SpriteRenderer>().enabled = true;
			yield return new WaitForSeconds(.2f);
			StartCoroutine(Blink());
		}
	}

	//-------------------------------------------
	private string aux;
	//------------------------------------------

	void OnCollisionEnter2D(Collision2D col){
		//Scenary Objects
		if (col.gameObject.tag.Equals("Ground")) {
			anim.SetBool ("isGrounded", true);
			anim.SetBool ("isJumping", false);
		}
		if (col.gameObject.tag.Equals("NextLevel")) {
			col.gameObject.GetComponent<LevelController>().Load();
		}

		//IA Objects
		if(col.gameObject.tag.Equals("Enemy")){
			this.Knockback(0);
		}
	}

	void OnCollisionExit2D(Collision2D col){
		if (col.gameObject.tag == "Ground")
			anim.SetBool ("isGrounded", false);
	}
	
	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag.Equals ("Stairs")) {
			if (Input.GetAxisRaw ("Vertical") != 0) {
				transform.Translate (new Vector3 (0, Input.GetAxisRaw ("Vertical") * Time.deltaTime, 0));
				rig.gravityScale = 0;
				rig.mass = 100;
			}
		}
	}

	void OnTriggerExit2D(Collider2D col){
		rig.gravityScale = 1;
		rig.mass = 1;
	}

	//GETTERS AND SETTERS--------------------------------------------------------

	public void SetActualMechanic(Mechanic m){
		if (m == Mechanic.NORMAL) {
			rig.gravityScale = 1f;
			colliding = null;
			actualMechanic = m;
		} else {
			actualMechanic = m;
		}
	}
	public Mechanic GetActualMechanic(){
		return actualMechanic;
	}

	public int GetActualLife(){
		return this.life;
	}

}

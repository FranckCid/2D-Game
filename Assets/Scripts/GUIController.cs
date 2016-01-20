using UnityEngine;
using System.Collections;

public class GUIController : MonoBehaviour {

	public Vector3 healthbarPosition;

	[SerializeField]
	private GameObject healthPref;
	[SerializeField]
	private GameObject halfHealthPref;

	private GameObject player;
	private int playerLife;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	private int aux;
	void Update () {
		playerLife = player.GetComponent<PlayerBehaviour> ().GetActualLife ();
		if (aux != playerLife) {
			this.ClearScreen();
			int i=0;
			for (i=0; i<playerLife;){
				i+=5;
				Vector3 where = Camera.main.ScreenToWorldPoint (new Vector3 (healthbarPosition.x + i, -Screen.height - healthbarPosition.y, 0.10f+i/10));
				GameObject hearth = Instantiate (healthPref, where, Quaternion.Euler (Vector3.zero)) as GameObject;
				hearth.transform.parent = this.transform;
				/*if(i-playerLife>=1){ //USE ME IF YOU WANT HALF HEARTHS
					GameObject halfhearth = Instantiate (halfHealthPref, where, Quaternion.Euler (Vector3.zero)) as GameObject;
					halfhearth.transform.parent = this.transform;
				}*/
			}

		}
		aux = playerLife;
	}
	
	void OnGUI(){
		GUILayout.Label(playerLife.ToString());
	}

	void ClearScreen(){
		foreach(GameObject obj in GameObject.FindGameObjectsWithTag("GUI")){
			Debug.Log(obj.name);
			Destroy(obj);
		}
	}

}

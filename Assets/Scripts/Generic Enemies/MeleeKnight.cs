using UnityEngine;
using System.Collections;

public class MeleeKnight : EnemyBehaviour {

	[SerializeField]
	private Sprite[] sprite;

	void Start () {
		GetComponentInChildren<SpriteRenderer> ().sprite = sprite[Random.Range(0, sprite.Length)];
	}
}

using UnityEngine;
using System.Collections;

public enum AttackType{
	MELEE,
	RANGED
}

public abstract class CharacterBase : MonoBehaviour {

	[SerializeField]
	protected int life;
	[SerializeField]
	protected int strenght;
	[SerializeField]
	protected float range;

	protected abstract void Attack ();
	public abstract void Damage(int d);
	public abstract void Knockback(float q);

}

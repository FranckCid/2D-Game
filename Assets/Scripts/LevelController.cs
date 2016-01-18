using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	[SerializeField]
	private string levelToLoad;

	public void Load(){
		Application.LoadLevel (levelToLoad);
	}

}

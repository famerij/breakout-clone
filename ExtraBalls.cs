using UnityEngine;
using System.Collections;

public class ExtraBalls : MonoBehaviour {

	// Sprites for different amount of extra balls left
	public Sprite[] sprites;

	private int index;
	
	void Start () {
		index = 3;
		gameObject.GetComponent<SpriteRenderer> ().sprite = sprites[index];
	}

	public void DecraseExtraBalls() {
		index--;
		gameObject.GetComponent<SpriteRenderer> ().sprite = sprites[index];
	}
}

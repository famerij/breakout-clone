using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {
	
	void OnMouseUp() {
		Application.LoadLevel("breakout-clone");
	}
}

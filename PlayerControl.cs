using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	private Vector3 mousePosition;
	public float moveSpeed = 1f;
	private static float playerYPos = -4f;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		transform.position = Vector2.Lerp(new Vector2(transform.position.x,playerYPos), new Vector2(mousePosition.x,playerYPos), moveSpeed);

		// restrict the player from moving outside camera
		if (transform.position.x <= Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x + GetComponent<Renderer>().bounds.size.x / 2) {
			transform.position = new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x + GetComponent<Renderer>().bounds.size.x / 2, playerYPos);
		} else if (transform.position.x >= Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x - GetComponent<Renderer>().bounds.size.x / 2) {
			transform.position = new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x - GetComponent<Renderer>().bounds.size.x / 2, playerYPos);
		}
	}
}

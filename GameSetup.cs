using UnityEngine;
using System.Collections;

public class GameSetup : MonoBehaviour {

	public Camera MainCamera;
	public BoxCollider2D topWall;
	public BoxCollider2D bottomWall;
	public BoxCollider2D leftWall;
	public BoxCollider2D rightWall;

	// Prefabs
	public GameObject blockPrefab;
	public GameObject player;
	public GameObject ballPrefab;

	public int extraBalls;

	private int blockCount;
	
	void Start () {
		// Move walls to screen edges
		topWall.size = new Vector2(MainCamera.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f);
		topWall.offset = new Vector2(0f, MainCamera.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y + 0.5f);

		bottomWall.size = new Vector2(MainCamera.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f);
		bottomWall.offset = new Vector2(0f, MainCamera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).y - 0.5f);
		bottomWall.tag = "Bottom";
		bottomWall.isTrigger = true;

		leftWall.size = new Vector2(1f, MainCamera.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).y);
		leftWall.offset = new Vector2(MainCamera.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x - 0.5f, 0f);

		rightWall.size = new Vector2(1f, MainCamera.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).y);
		rightWall.offset = new Vector2(MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x + 0.5f, 0f);

		//Set up blocks
		//first position
		Vector3 firstBlockPos = new Vector3(-7f,4f);
		float xIncrement = blockPrefab.GetComponent<Renderer>().bounds.size.x / 2 + 1f;
		float yIncrement = blockPrefab.GetComponent<Renderer>().bounds.size.y / 2 + .5f;

		// Instantiate blocks
		blockCount = 0;
		//rows
		for (int j = 0; j > -5; j--) {
			//cols
			for (int i = 0; i < 9; i++) {
				GameObject block;
				block = Instantiate(blockPrefab,new Vector3(firstBlockPos.x + i * xIncrement, firstBlockPos.y + j * yIncrement),new Quaternion()) as GameObject;
				block.GetComponent<SpriteRenderer>().color = new Color(0.0f, 0.5f, 0.6f * i * 0.15f);

				blockCount++;
			}
		}

		// Instantiate the ball
		SpawnBall();
	}

	void SpawnBall() {
		GameObject ball;
		ball = Instantiate (ballPrefab, new Vector3 (), new Quaternion()) as GameObject;
	}

	public void CheckExtraBalls() {
		if (extraBalls <= 0) {
			Application.LoadLevel("menu");
		}
		else {
			GameObject extraBallsUI = GameObject.Find ("ExtraBallsUIElem");
			ExtraBalls uiScript = (ExtraBalls) extraBallsUI.GetComponent (typeof(ExtraBalls));
			uiScript.DecraseExtraBalls();
			extraBalls--;
			SpawnBall();
			Debug.Log (extraBalls);
		}
	}

	public void BlockDestroyed() {
		blockCount--;
		if (blockCount == 0) {
			Application.LoadLevel("menu");
		}
	}
}

using UnityEngine;
using System.Collections;

public class BlockDestroy : MonoBehaviour {

	// Send information of broken blocks to the main camera
	// The GameSetup script counts if there is any blocks left in the level

	private GameObject mainCamera;
	private GameSetup script;
	
	void Start () {
		mainCamera = GameObject.Find("MainCamera");
		script = (GameSetup) mainCamera.GetComponent (typeof(GameSetup));
	}
	
	public void Destroy() {
		script.BlockDestroyed();

		// Making so that the block doesn't react to the ball anymore when hit
		gameObject.GetComponent<Collider2D>().enabled = false;
		gameObject.GetComponent<Rigidbody2D>().isKinematic = true;

		// Scaling down animation on destruction
		StartCoroutine(ScaleDown());
	}

	public IEnumerator ScaleDown() {
		Vector3 fromScale = transform.localScale;
		float duration = 0.5f;
		for (float t=0f;t<duration;t+=Time.deltaTime) {
			transform.localScale = Vector3.Lerp(fromScale, new Vector3(), t/duration);
			yield return 0;
		}
		Destroy(gameObject);
	}
}

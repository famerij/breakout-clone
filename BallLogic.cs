using UnityEngine;
using System.Collections;

public class BallLogic : MonoBehaviour {
	
	// Speed set in editor
	public float speed;

	// Different sound effects
	public AudioClip blockImpact;
	public AudioClip playerImpact;
	public AudioClip wallImpact;
	public AudioClip destruction;

	// Keep track of the direction in which the ball is moving
	private Vector2 velocity;
	
	// used for velocity calculation
	private Vector2 lastPos;

	private GameObject player;

	// GameSetup script access
	private GameObject mainCamera;
	private GameSetup setupScript;
	
	void Start () {
		// Random direction
		GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle.normalized * speed;
		// Set the player
		player = GameObject.Find("Player");
		// Set camera for global game setup script
		mainCamera = GameObject.Find ("MainCamera");
		setupScript = (GameSetup) mainCamera.GetComponent (typeof(GameSetup));
	}

	void FixedUpdate () {
		// Get pos 2d of the ball.
		Vector3 pos3D = transform.position;
		Vector2 pos2D = new Vector2(pos3D.x, pos3D.y);
		
		// Velocity calculation. Will be used for the bounce
		velocity = pos2D - lastPos;
		lastPos = pos2D;

		// Preventing the ball from stopping in corners, cheap fix I know
		if (GetComponent<Rigidbody2D>().velocity.magnitude < speed / 2) {
			GetComponent<Rigidbody2D>().velocity = new Vector2(-1f,1f) * speed;
		}
	}
	
	void OnCollisionEnter2D(Collision2D coll) {

		// If hit by player, set the velocity according to the angle in which the palyer was hit
		if (coll.collider.transform.tag == "Player") {
			Vector2 playerPos = player.transform.position;
			
			// The vector from player to the ball
			Vector2 delta = lastPos - playerPos;
			
			// Normalized vector
			Vector2 direction = delta.normalized;

			//Debug.DrawRay(lastPos, direction, Color.yellow, 3f);
			
			// Set the velocity to be the direction vector scaled to the constant speed
			GetComponent<Rigidbody2D>().velocity = direction * speed;

			// Playing sound effect, randomizing pitch for variation
			GetComponent<AudioSource>().pitch = Random.Range (0.8f,1f);
			GetComponent<AudioSource>().PlayOneShot(playerImpact);
		}

		// Bounce from anywhere else
		else {
			// Normal
			Vector3 N = coll.contacts[0].normal;
			
			// Direction
			Vector3 V = velocity.normalized;
			
			// Reflection
			Vector3 R = Vector3.Reflect(V, N).normalized;

			//Debug.DrawRay(lastPos, R, Color.white, 3f);

			// Assign normalized reflection with the constant speed
			GetComponent<Rigidbody2D>().velocity = new Vector2(R.x, R.y) * speed;

			// Playing sound effect
			// Block impacts give different sound
			if (coll.gameObject.tag != "Block") {
				GetComponent<AudioSource>().pitch = Random.Range (0.8f,1f);
				GetComponent<AudioSource>().PlayOneShot(wallImpact);
			}
		}
		if (coll.gameObject.tag == "Block") {
			BlockDestroy destroyScript;
			destroyScript = (BlockDestroy) coll.gameObject.GetComponent (typeof(BlockDestroy));
			destroyScript.Destroy();

			// Playing sound effect
			GetComponent<AudioSource>().pitch = Random.Range (0.6f,1f);
			GetComponent<AudioSource>().PlayOneShot(blockImpact);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		// Detect when the bottom is hit
		if (other.tag == "Bottom") {
			StartCoroutine(DestroyBall());
		}
	}

	IEnumerator DestroyBall() {
		// Playing sound effect
		GetComponent<AudioSource>().PlayOneShot(destruction);
		yield return new WaitForSeconds(destruction.length);
		
		Destroy(gameObject);

		setupScript.CheckExtraBalls();
	}
}

using UnityEngine;
using System.Collections;

using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour {

	private Camera camera;
	public float playerRotation = 0;
	public float maxSpeed = 0;
	public float speed = 0;
	public float jumpPower = 0;
	float gravity;
	public bool grounded;
	public Rigidbody2D playerRigidbody;
	private Animator anim;
	public Vector3 velocity = new Vector3();
	public bool dead;

	public float time;
	public float time0;
	public float displacment;
	float dt;


	// Death stuff
	public Texture deathTexture;
	private bool deadStage = false;
	private float deathStart = 0;
	private float deathTime = 3.0f;
	private float alphaFade;


	// Music
	GameObject musicPlayer;


	// Respawning/checkpoints
	public GameObject[] respawnLocs;
	private Vector3 respawn;
	private int respawnNum;

	//v = g*t;
	//d = d0 + g*t*t/2 


	void Start () 
	{
		camera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
		playerRigidbody = gameObject.GetComponent<Rigidbody2D> ();
		anim = gameObject.GetComponent<Animator> ();
		musicPlayer = GameObject.FindGameObjectWithTag("Music");

		//respawnLocs = GameObject.FindGameObjectsWithTag ("Respawn");
		respawnNum = -1;
	}

	void OnGUI() {
		if (dead) {
			// Update death stuff
			if (deadStage) {
				alphaFade += Mathf.Clamp01 (Time.deltaTime / deathTime);
				GUI.color = new Color (0, 0, 0, alphaFade);
				GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), deathTexture);
			} else {
				alphaFade -= Mathf.Clamp01 (Time.deltaTime / deathTime);
				GUI.color = new Color (0, 0, 0, alphaFade);
				GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), deathTexture);
			}
		}
	}

	void Update () 
	{ 

		if (Input.GetKey(KeyCode.Escape)) {
			Application.Quit();
		}

		if (dead) {
			playerRigidbody.velocity = new Vector2(0,0);

			if (deathStart == 0) {
				deadStage = true;
				deathStart = Time.realtimeSinceStartup;
				alphaFade = 0;

				musicPlayer.GetComponent<Music>().selectMusic(1);

				anim.SetBool("Dead", true);
			}

			if (Time.realtimeSinceStartup - deathStart > deathTime / 2 && deadStage) {
				transform.position = respawn;

				if (respawnNum == 0 || respawnNum == 1) {
					playerRotation = 0;
					camera.transform.localEulerAngles = new Vector3 (0, 0, 0);
				} else if (respawnNum == 2 || respawnNum == 3) {
					playerRotation = 90;
					camera.transform.localEulerAngles = new Vector3(0, 0, 90);
				} else if (respawnNum == 4) {
					playerRotation = 270;
					camera.transform.localEulerAngles = new Vector3(0, 0, 270);
				}

				deadStage = false;
			}

			if (Time.realtimeSinceStartup - deathStart > deathTime) {
				deathStart = 0;
				anim.SetBool("Dead", false);
				dead = false;
			}

		} else {


			//checkBox (new Vector2(40, -6), new Vector2(82, 205), 90);
			//checkBox (new Vector2(10, 205), new Vector2(60, 215), 180);

			transform.localEulerAngles = new Vector3 (0, 0, playerRotation);

			if (!grounded)
				time++;

			if (playerRotation == 0) {
				UpdatePosY ();
			} else if (playerRotation == 90) {
				UpdatePosX ();
			} else if (playerRotation == 180) {
				UpdateNegY ();
			} else if (playerRotation == 270) {
				UpdateNegX ();
			}


			// Camera Stuff!
			int playerRot = (int)playerRotation;

			Vector2 newUp = new Vector2 (0, 0);
			switch (playerRot) {
			case 0:
				newUp = new Vector2 (0, 1);
				break;
			case 90:
				newUp = new Vector2 (-1, 0);
				break;
			case 180:
				newUp = new Vector2 (0, -1);
				break;
			case 270:
				newUp = new Vector2 (1, 0);
				break;
			}

			Vector2 camRot = camera.transform.up;
			Vector2 newCamRot = Vector2.Lerp (camRot, newUp, Time.deltaTime * 8f);
			camera.transform.up = new Vector3 (newCamRot.x, newCamRot.y, 0);

			//camera.transform.localEulerAngles = new Vector3(0, 0, 90);

			// Some really, really janky fixed for Position #4 (REALLY!)
			Quaternion q = camera.transform.rotation;
			if (q.x == 1.0 && q.z == 0.0)
				camera.transform.rotation = new Quaternion (0, 0, 1, 0);
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag ("Grav")) {
			playerRotation = col.GetComponentInParent<GravTrigger> ().upAngle;
			camera.transform.Rotate (new Vector3 (0, 0, 1));

			musicPlayer.GetComponent<Music> ().selectMusic (col.GetComponentInParent<GravTrigger> ().song);
		} else if (col.CompareTag ("Checkpoint") && !col.GetComponentInParent<Checkpoints> ().isTriggered ()) {
			col.GetComponentInParent<Checkpoints> ().trigger ();

			respawn = respawnLocs [++respawnNum].transform.position;
		} else if (col.CompareTag ("Elevator")) {
			GameObject.Find("Elevator").GetComponent<ElevatorRun>().startElevator();
		}
	}

	void checkBox(Vector2 p1, Vector2 p2, int playerRot){
		camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera> ();
		
		if (transform.position.x > p1.x && transform.position.x < p2.x && transform.position.y > p1.y && transform.position.y < p2.y) {
			playerRotation = playerRot % 360;

			//if (playerRot != 90)
			
			Vector2 newUp = new Vector2(0,0);
			switch (playerRot)
			{
			case 0:
				newUp = new Vector2(0, 1);
				break;
			case 90:
				newUp = new Vector2(-1, 0);
				break;
			case 180:
				newUp = new Vector2(0, -1);
				break;
			case 270:
				newUp = new Vector2(1, 0);
				break;
			}
			
			Vector2 camRot = camera.transform.up;
			Vector2 newCamRot = Vector2.Lerp (camRot, newUp, Time.deltaTime * 4f);
			camera.transform.up = new Vector3 (newCamRot.x, newCamRot.y, 0);
			
			//camera.transform.localEulerAngles = new Vector3(0, 0, 90);

			// Some really, really janky fixed for Position #4 (REALLY!)
			Quaternion q = camera.transform.rotation;
			if (q.x == 1.0 && q.z == 0.0)
				camera.transform.rotation = new Quaternion (0, 0, 1, 0);
		}
	}

	void UpdatePosY()
	{

		/*****SET GRAVITY******/
		gravity = -5f;

		/*****GET AXIS******/
		float h = CrossPlatformInputManager.GetAxis ("Horizontal");
		float v = CrossPlatformInputManager.GetAxis ("Vertical");


		/*****ANIMATION STUFF*****/
		anim = gameObject.GetComponent<Animator> ();
		anim.SetBool ("Grounded", grounded);

		/*****PLAYER DIRECTION*****/
		if (CrossPlatformInputManager.GetAxis ("Horizontal") < -0.1f) {
			transform.localScale = new Vector3 (-1, 1, 1);
			anim.SetFloat ("Speed", 1);
		} else if (CrossPlatformInputManager.GetAxis ("Horizontal") > 0.1f) {
			transform.localScale = new Vector3 (1, 1, 1);
			anim.SetFloat ("Speed", 1);
		} else {
			anim.SetFloat ("Speed", 0);
		}
		
		/*****GRAVITY CODE*****/
		velocity.y = gravity * time;
		//displacment = velocity.y*time;
		
		playerRigidbody.AddForce (Vector2.up * velocity.y);


		/*****INITIALIZE EASING VECTOR******/
		Vector3 easeVelocity = new Vector3 ();
		easeVelocity.z = 0.0f;
		easeVelocity.y = velocity.y;// + gravity * time;
		easeVelocity.x *= 0.75f;

		/*****MOVE PLAYER RIGHT******/

		playerRigidbody.AddForce ((Vector2.right * speed) * h);
		
		if (grounded) 
		{
			playerRigidbody.velocity = easeVelocity;
		}


		/*****PLAYER JUMP******/
		if (CrossPlatformInputManager.GetButtonDown ("Jump") && grounded) {
			playerRigidbody.AddForce(Vector2.up * jumpPower);
		}

		/*****SLOW DOWN MOVEMENT*****/
		if (playerRigidbody.velocity.x > maxSpeed) {
			playerRigidbody.velocity = new Vector2(maxSpeed, playerRigidbody.velocity.y);
		}
		if (playerRigidbody.velocity.x < -maxSpeed) {
			playerRigidbody.velocity = new Vector2(-maxSpeed, playerRigidbody.velocity.y);
		}


	}

	void UpdatePosX(){
		/*****SET GRAVITY******/
		gravity = -5f;
		
		/*****GET AXIS******/
		float h = CrossPlatformInputManager.GetAxis ("Horizontal");
		float v = CrossPlatformInputManager.GetAxis ("Vertical");
		
		
		/*****ANIMATION STUFF*****/
		anim = gameObject.GetComponent<Animator> ();
		anim.SetBool ("Grounded", grounded);

		/*****PLAYER DIRECTION*****/
		if (CrossPlatformInputManager.GetAxis ("Horizontal") < -0.1f) {
			transform.localScale = new Vector3 (-1, 1, 1);
			anim.SetFloat ("Speed", 1);
		} else if (CrossPlatformInputManager.GetAxis ("Horizontal") > 0.1f) {
			transform.localScale = new Vector3 (1, 1, 1);
			anim.SetFloat ("Speed", 1);
		} else {
			anim.SetFloat ("Speed", 0);
		}
		
		/*****GRAVITY CODE*****/
		velocity.x = gravity * time;
		//displacment 4    = velocity.x*time;

		playerRigidbody.AddForce (new Vector2(-1,0) * velocity.x);
		
		
		/*****INITIALIZE EASING VECTOR******/
		Vector3 easeVelocity = new Vector3 ();
		easeVelocity.z = 0.0f;
		easeVelocity.x = velocity.x;// + gravity * time;
		easeVelocity.y *= 0.75f;
		
		/*****MOVE PLAYER RIGHT******/
		
		playerRigidbody.AddForce ((new Vector2(0,1) * speed) * h);
		
		if (grounded) 
		{
			playerRigidbody.velocity = easeVelocity;
		}
		
		
		/*****PLAYER JUMP******/
		if (CrossPlatformInputManager.GetButtonDown ("Jump") && grounded) {
			playerRigidbody.AddForce(new Vector2(-1,0) * jumpPower);
		}

		/*****SLOW DOWN MOVEMENT*****/
		if (playerRigidbody.velocity.y > maxSpeed) {
			playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, maxSpeed);
		}
		if (playerRigidbody.velocity.y < -maxSpeed) {
			playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, -maxSpeed);
		}



	}

	void UpdateNegX(){
		/*****SET GRAVITY******/
		gravity = -5f;

		float h = CrossPlatformInputManager.GetAxis ("Horizontal");
		float v = CrossPlatformInputManager.GetAxis ("Vertical");
		
		
		/*****ANIMATION STUFF*****/
		anim = gameObject.GetComponent<Animator> ();
		anim.SetBool ("Grounded", grounded);

		/*****PLAYER DIRECTION*****/
		if (CrossPlatformInputManager.GetAxis ("Horizontal") < -0.1f) {
			transform.localScale = new Vector3 (-1, 1, 1);
			anim.SetFloat ("Speed", 1);
		} else if (CrossPlatformInputManager.GetAxis ("Horizontal") > 0.1f) {
			transform.localScale = new Vector3 (1, 1, 1);
			anim.SetFloat ("Speed", 1);
		} else {
			anim.SetFloat ("Speed", 0);
		}
		
		/*****GRAVITY CODE*****/
		velocity.x = gravity * time;
		//displacment = velocity.x*time;
		
		playerRigidbody.AddForce (new Vector2(1,0) * velocity.x);
		
		
		/*****INITIALIZE EASING VECTOR******/
		Vector3 easeVelocity = new Vector3 ();
		easeVelocity.z = 0.0f;
		easeVelocity.x = velocity.x;// + gravity * time;
		easeVelocity.y *= 0.75f;
		
		/*****MOVE PLAYER RIGHT******/
		
		playerRigidbody.AddForce ((new Vector2(0,-1) * speed) * h);
		
		if (grounded) 
		{
			playerRigidbody.velocity = easeVelocity;
		}
		
		
		/*****PLAYER JUMP******/
		if (CrossPlatformInputManager.GetButtonDown("Jump") && grounded) {
			playerRigidbody.AddForce(new Vector2(1, 0) * jumpPower);
		}
		
		/*****SLOW DOWN MOVEMENT*****/
		if (playerRigidbody.velocity.y > maxSpeed) {
			playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, maxSpeed);
		}
		if (playerRigidbody.velocity.y < -maxSpeed) {
			playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, -maxSpeed);
		}

	}

	void UpdateNegY(){
		
		transform.localEulerAngles = new Vector3(0, 0, 180);
		/*****SET GRAVITY******/
		gravity = -5f;
		
		/*****GET AXIS******/
		float h = CrossPlatformInputManager.GetAxis ("Horizontal");
		float v = CrossPlatformInputManager.GetAxis ("Vertical");
		
		
		/*****ANIMATION STUFF*****/
		anim = gameObject.GetComponent<Animator> ();
		anim.SetBool ("Grounded", grounded);

		/*****PLAYER DIRECTION*****/
		if (CrossPlatformInputManager.GetAxis ("Horizontal") < -0.1f) {
			transform.localScale = new Vector3 (-1, 1, 1);
			anim.SetFloat ("Speed", 1);
		} else if (CrossPlatformInputManager.GetAxis ("Horizontal") > 0.1f) {
			transform.localScale = new Vector3 (1, 1, 1);
			anim.SetFloat ("Speed", 1);
		} else {
			anim.SetFloat ("Speed", 0);
		}
		
		/*****GRAVITY CODE*****/
		velocity.y = gravity * time;
		//displacment = velocity.y*time;
		
		playerRigidbody.AddForce (new Vector2(0,-1) * velocity.y); 
		
		
		/*****INITIALIZE EASING VECTOR******/
		Vector3 easeVelocity = new Vector3 ();
		easeVelocity.z = 0.0f;
		easeVelocity.y = velocity.y;// + gravity * time;
		easeVelocity.x *= 0.75f;
		
		/*****MOVE PLAYER RIGHT******/
		
		playerRigidbody.AddForce ((new Vector2(-1,0) * speed) * h);
		
		if (grounded) 
		{
			playerRigidbody.velocity = easeVelocity;
		}
		
		
		/*****PLAYER JUMP******/
		if (CrossPlatformInputManager.GetButtonDown ("Jump") && grounded) {
			playerRigidbody.AddForce(new Vector2(0, -1) * jumpPower);
		}
		
		/*****SLOW DOWN MOVEMENT*****/

		if (playerRigidbody.velocity.x > maxSpeed) {
			playerRigidbody.velocity = new Vector2(maxSpeed, playerRigidbody.velocity.y);
		}
		if (playerRigidbody.velocity.x < -maxSpeed) {
			playerRigidbody.velocity = new Vector2(-maxSpeed, playerRigidbody.velocity.y);
		}


	}
}

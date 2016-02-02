using UnityEngine;
using System.Collections;

public class End : MonoBehaviour {

	public Transform end;
	private Player player;
	private Camera cam;

	private bool startZoom, startCount;
	public float maxFOV;
	public int zoomSpeed;

	private float startTime;

	void Start(){
		cam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player> ();

		startZoom = false;
	}

	void Update() {
		if (startZoom) {
			cam.orthographicSize += (zoomSpeed * Time.deltaTime);

			if (cam.orthographicSize > maxFOV) {
				cam.orthographicSize = maxFOV;
				startZoom = false;
			}
		}

		if (startCount) {
			if (Time.realtimeSinceStartup - startTime > 1) {
				Vector3 x = end.position;
				end.position = new Vector3(428f, x.y, x.z);
			}

			if (Time.realtimeSinceStartup - startTime > 7) {
				Application.Quit ();
			}
		}
	}
	
	void OnTriggerEnter2D(Collider2D col){
		
		if (col.CompareTag ("Player")) {
			startTime = Time.realtimeSinceStartup;
			startZoom = true;
			startCount = true;
		}
		
	}
}

using UnityEngine;
using System.Collections;

public class ElevatorRun : MonoBehaviour {

	public Transform end;
	public float speed;
	private bool running, finished;
	float tempx;
	float tempy;

	// Use this for initialization
	void Start () {
		running = false;
		finished = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (running && !finished) {

			Vector3 movement = end.position - transform.position;// new Vector2(0, 1);//end.position - transform.position;
			tempx = transform.position.x;
			tempy = transform.position.y;
			//transform.position.x = tempy;
			//transform.position.y = tempx;
			movement.Normalize();
			movement *= speed * Time.deltaTime;
			transform.Translate(movement);

			if ((end.position - transform.position).magnitude < 5) {
				finished = true;
			}
		}
	}

	public void startElevator() {
		running = true;
	}
}

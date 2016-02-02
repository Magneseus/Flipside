using UnityEngine;
using System.Collections;

public class Swoop : MonoBehaviour {

	public Transform[] points;
	public float speed, totalTime;

	private float curSpeed;
	private int stage, curPoint;

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		curSpeed = 0;
		stage = 0;
		curPoint = 1;
	}
	
	// Update is called once per frame
	void Update () {

		float newSpeed = Mathf.Clamp01(speed * Time.deltaTime / totalTime);
		curSpeed += newSpeed;
		curSpeed = curSpeed > speed ? speed : curSpeed;

		if (stage == 0) {
			Vector2 momentum = points[curPoint].position - rb.transform.position;
			momentum *= curSpeed;

			rb.velocity = momentum;
			//rb.AddForce(momentum);

			if ((points[curPoint].position - rb.transform.position).magnitude < 0.1) {
				curSpeed = 0;
				curPoint++;
			}

			if (curPoint == 4) {
				switchStage();
			}
		} else {
			Vector2 momentum = points[curPoint].position - rb.transform.position;
			momentum *= curSpeed;

			rb.velocity = momentum;
			//rb.AddForce(momentum);
			
			if ((points[curPoint].position - rb.transform.position).magnitude < 0.1) {
				curSpeed = 0;
				curPoint--;
			}
			
			if (curPoint == -1) {
				switchStage();
			}
		}
	}

	void switchStage() {
		if (stage == 0) {
			stage = 1;
			curSpeed = 0;
			curPoint = 2;
		} else {
			stage = 0;
			curSpeed = 0;
			curPoint = 1;
		}
	}
}

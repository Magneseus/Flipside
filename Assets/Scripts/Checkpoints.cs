using UnityEngine;
using System.Collections;

public class Checkpoints : MonoBehaviour {

	public Sprite unlit, lit;

	private bool triggered;

	// Use this for initialization
	void Start () {
		triggered = false;
		GetComponent<SpriteRenderer> ().sprite = unlit;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool isTriggered() {
		return triggered;
	}

	public void trigger() {
		triggered = true;
		GetComponent<SpriteRenderer> ().sprite = lit;
	}
}

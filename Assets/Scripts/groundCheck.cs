using UnityEngine;
using System.Collections;

public class groundCheck : MonoBehaviour {

	private Player player;

	void Start()
	{
		player = gameObject.GetComponentInParent<Player> ();
	}
	void update()
	{
	
	}
	/*
	void OnTriggerEnter2D(Collider2D col)
	{
		string groundTag;
		if (player.playerRotation == 0) {
			groundTag = "down";
		}else if (player.playerRotation == 90) {
			groundTag = "left";
		}else if (player.playerRotation == 180) {
			groundTag = "up";
		}else if (player.playerRotation == 270) {
			groundTag = "right";
		}
		if (col.gameObject.tag == groundTag) {
			player.grounded = true;
			player.time = 0;
		}
	}
*/
	void OnTriggerStay2D(Collider2D col)
	{
		if (!col.CompareTag ("Grav") && !col.CompareTag("Respawn") && !col.CompareTag("Checkpoint") && !col.CompareTag("StopAI")) {
			player.grounded = true;
			player.time = 0;
		}
	}


	void OnTriggerExit2D(Collider2D col)
	{
		if (!col.CompareTag ("Grav") && !col.CompareTag("Respawn") && !col.CompareTag("Checkpoint") && !col.CompareTag("StopAI")) {
			player.grounded = false;
			player.time = 0;
		}
	}
}

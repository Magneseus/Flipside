using UnityEngine;
using System.Collections;

public class ColliderStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnTriggerEnter2D(Collider2D col){
		Application.LoadLevel ("gamejam");
	}
}

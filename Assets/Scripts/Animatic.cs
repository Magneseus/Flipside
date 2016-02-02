using UnityEngine;
using System.Collections;

public class Animatic : MonoBehaviour {

	IEnumerator Start() {
		yield return new WaitForSeconds(20);
		this.transform.position = new Vector3 (100,100,100);
	}
}

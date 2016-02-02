using UnityEngine;
using System.Collections;

public class particles : MonoBehaviour {
	public float blinker;
	private int time;

	// Update is called once per frame
	void Update () {

		foreach (SpriteRenderer g in GetComponentsInChildren<SpriteRenderer>())
		{
			Color c = g.color;
			g.color = new Color (c.r, c.g, c.b, (0.5f + (Mathf.Sin (Mathf.Deg2Rad * time * blinker))/2));
			time++;
		}
	}
}

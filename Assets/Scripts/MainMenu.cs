using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Texture mainScreen;

	public Texture story;
	public Texture quick;
	public Texture exit;

	private Rect storyRect;
	private Rect quickRect;
	private Rect exitRect;

	// Use this for initialization
	void Start () {
		storyRect = new Rect (Screen.width * 0.06f, Screen.height * 0.40f, Screen.width * 0.24f, Screen.height * 0.14f);
		quickRect = new Rect (Screen.width * 0.06f, Screen.height * 0.58f, Screen.width * 0.24f, Screen.height * 0.14f);
		exitRect = new Rect (Screen.width * 0.06f, Screen.height * 0.76f, Screen.width * 0.24f, Screen.height * 0.14f);
	}
	
	// Update is called once per frame
	void Update () {

		storyRect = new Rect (Screen.width * 0.06f, Screen.height * 0.40f, Screen.width * 0.24f, Screen.height * 0.14f);
		quickRect = new Rect (Screen.width * 0.06f, Screen.height * 0.58f, Screen.width * 0.24f, Screen.height * 0.14f);
		exitRect = new Rect (Screen.width * 0.06f, Screen.height * 0.76f, Screen.width * 0.24f, Screen.height * 0.14f);

		/*
		if (Input.GetMouseButtonDown (0)) {
			Vector2 mousePos = Input.mousePosition;

			if (storyRect.Contains(mousePos)) {

			}
			if (quickRect.Contains(mousePos)) {
				print ("TEST");

			}
			if (exitRect.Contains(mousePos)) {
				print ("TEST");

			}
		}*/
	}

	void OnGUI() {
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), mainScreen);

	 	if (GUI.Button (storyRect, story)) {
			Application.LoadLevel("intro");
		}
		if (GUI.Button (quickRect, quick)) {
			Application.LoadLevel ("gamejam");
		}
		if (GUI.Button (exitRect, exit)) {
			Application.Quit ();
		}
	}
}

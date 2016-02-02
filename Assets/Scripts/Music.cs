using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {
	AudioSource[] audioList;

	// Use this for initialization
	void Start () {
		audioList = GetComponentsInChildren<AudioSource> ();

	}

	public void selectMusic(int audioNum){
		for (int i = 0; i < 4; i++) {
			audioList[i].mute = true;
		}

		if (audioNum != 5)
			audioList [5 - audioNum - 1].mute = false;
		else
			audioList [4].mute = false;
	}


	// Update is called once per frame
	void Update () {
	
	}
}

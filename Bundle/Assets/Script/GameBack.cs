using UnityEngine;
using System.Collections;

public class GameBack : MonoBehaviour {

	public string levelName = "Main";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		if (GUI.Button (new Rect(10, 10, 100, 30), "< Back")) {
			Application.LoadLevel (levelName);
		}
	}
}

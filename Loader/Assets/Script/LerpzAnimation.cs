using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animation))]
public class LerpzAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!this.animation.isPlaying) {
			this.animation.Play();
		}
	}
}

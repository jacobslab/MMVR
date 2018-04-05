using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPlayer : MonoBehaviour {
	public Animator animController;
	public AudioSource aud;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.P)) {
			PlayAnimation ();
		}
		
	}

	void PlayAnimation()
	{
		animController.Play ("Gettysburg");
		aud.Play ();
	}
}

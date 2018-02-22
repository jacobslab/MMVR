using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandboxEditorManager : MonoBehaviour {

	public Camera sandBoxCamera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ToggleCamera(bool shouldEnable)
	{
		Debug.Log (" should enable: " + shouldEnable.ToString ());
		sandBoxCamera.enabled = shouldEnable;
		sandBoxCamera.GetComponent<AudioListener> ().enabled = shouldEnable;
	}
}

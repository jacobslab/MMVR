﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandboxEditorManager : MonoBehaviour {

	public Camera sandBoxCamera;
	public CanvasGroup propertyPanelGroup;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TogglePropertyPanel(bool shouldEnable)
	{
		if (shouldEnable)
			propertyPanelGroup.alpha = 1f;
		else
			propertyPanelGroup.alpha = 0f;
	}

	public void ToggleCamera(bool shouldEnable)
	{
		sandBoxCamera.enabled = shouldEnable;
		sandBoxCamera.GetComponent<AudioListener> ().enabled = shouldEnable;
	}
}
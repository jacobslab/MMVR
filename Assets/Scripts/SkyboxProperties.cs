using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxProperties : MonoBehaviour {

	public Material[] skyboxMatArr;
	// Use this for initialization
	void Start () {

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeSkybox(int index)
	{
		RenderSettings.skybox = skyboxMatArr [index];
	}
}

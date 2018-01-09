using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControls : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mousePoint = Input.mousePosition;
		mousePoint.z = 10f;
		Vector3 worldPos = Camera.main.ScreenToWorldPoint (mousePoint);
		transform.RotateAround(worldPos, Vector3.up, worldPos.x * 0.1f); 
	}
}

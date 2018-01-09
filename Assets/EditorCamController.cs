using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCamController : MonoBehaviour {
	public static GameObject selectedObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {

			if (hit.collider != null) {
				Debug.Log ("Target Object " + hit.collider.gameObject.name);
			}
		}

	}

}

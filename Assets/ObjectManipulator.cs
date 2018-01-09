using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulator : MonoBehaviour {

	bool mouseDown=false;
	Color originalColor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (mouseDown)
			transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x,Input.mousePosition.y,10f));

		if (Input.GetMouseButton (1)) {
			float mouseX = Input.GetAxis ("Mouse X");
			float mouseY = Input.GetAxis ("Mouse Y");
			Debug.Log ("trying to do right click");
			Debug.Log (((int)(mouseY / 0.25f) * 90f) + " with mouseY : " + mouseY.ToString());
			transform.eulerAngles = new Vector3(transform.eulerAngles.x + ((int)(mouseY/0.25f) * 90f), transform.eulerAngles.y, transform.eulerAngles.z);
		}
	}

	void OnMouseDown()
	{
		
			originalColor = gameObject.GetComponent<Renderer> ().material.color;
			gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
			gameObject.GetComponent<BoxCollider> ().enabled = false;
		EditorCamController.selectedObj = this.gameObject;
			mouseDown = true;
	}
	void OnMouseUp()
	{
		gameObject.GetComponent<Renderer> ().material.color = originalColor;
		mouseDown = false;
		gameObject.GetComponent<BoxCollider> ().enabled = true;
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.layer == 8) {
			Physics.IgnoreCollision (col.collider, this.gameObject.GetComponent<BoxCollider> ());
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulator : MonoBehaviour {
	EditorCamController editorController { get { return EditorCamController.Instance; }}

	public SpawnableObject.ObjectType objType;
	bool mouseDown=false;
	Color originalColor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (mouseDown)
			transform.position = Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x,Input.mousePosition.y,10f));


	}

	void OnMouseDown()
	{
		
			originalColor = gameObject.GetComponent<Renderer> ().material.color;
			gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
			gameObject.GetComponent<BoxCollider> ().enabled = false;
			editorController.SetSelectedObject(this.gameObject);
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

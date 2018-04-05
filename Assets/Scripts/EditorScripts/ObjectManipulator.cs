using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ObjectManipulator : MonoBehaviour,IPointerDownHandler{
	EditorCamController editorController { get { return EditorCamController.Instance; }}

	public SpawnableObject.ObjectType objType;
	bool mouseDown=false;
	private float fixedDist=0f;
	Color originalColor;
	public float rotSpeed=2f;

	public bool selected=false;

	public EditorCamController.ObjectMode objMode;

	public enum MouseButtonState {
		Left,
		Right,
		Middle
	}
	private MouseButtonState mouseButtonState;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (selected) {
			if (mouseDown) {
				switch (objMode) {
				case EditorCamController.ObjectMode.Move:
//			Debug.Log ("fixed dist is: " + fixedDist.ToString());
					transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, fixedDist));
					break;
				case EditorCamController.ObjectMode.Rotate:
					OnSelection ();
					Vector3 testVec = new Vector3 (Input.GetAxis ("Mouse Y"), -Input.GetAxis ("Mouse X"), 0) * rotSpeed;
//			Debug.Log("rotating " + testVec.ToString());
					transform.Rotate (testVec);
					break;
				}
			}
		}
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log ("pointer: " + eventData.button.ToString ());
	}

	void OnMouseDown()
	{
		//	originalColor = gameObject.GetComponent<Renderer> ().material.color;
		//	gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
		OnSelection ();
		fixedDist=Vector3.Distance (transform.position, Camera.main.transform.position);
		selected = true;
		mouseDown = true;
	}

	void OnSelection()
	{
		editorController.objSelected = true;
		gameObject.GetComponent<BoxCollider> ().enabled = false;
		Debug.Log ("onselection: " + this.gameObject.name);
		editorController.SetSelectedObject(this.gameObject);
	}
	void OnDeselection()
	{
		EditorCamController.Instance.objSelected = false;
		gameObject.GetComponent<BoxCollider> ().enabled = true;
	}
	void OnMouseUp()
	{
	//	gameObject.GetComponent<Renderer> ().material.color = originalColor;
		mouseDown = false;
		OnDeselection ();
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.layer == 8) {
			Physics.IgnoreCollision (col.collider, this.gameObject.GetComponent<BoxCollider> ());
		}
	}
}

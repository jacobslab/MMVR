using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObjectController : MonoBehaviour {

	EditorCamController editorController { get { return EditorCamController.Instance; }}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RotateAnticlockwise()
	{
		GameObject selectedObj = editorController.GetSelectedObject ();
		if (selectedObj != null) {
			selectedObj.transform.Rotate(0,-90, 0);
		}
	}
	public void RotateClockwise()
	{
		GameObject selectedObj = editorController.GetSelectedObject ();
		if (selectedObj != null) {
			
			selectedObj.transform.Rotate(0, 90, 0);
		}
	}
}

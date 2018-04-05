using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectedObjectController : MonoBehaviour {

	EditorCamController editorController { get { return EditorCamController.Instance; }}

	public EditorCamController.ObjectMode objMode;
	public Button selectButton;
	public Button moveButton;
	public Button rotateButton;
	// Use this for initialization
	void Start () {
		SwitchToSelectMode ();
		selectButton.Select ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SwitchToSelectMode()
	{
		objMode =EditorCamController.ObjectMode.Select;
		DeselectAllButtons ();
		selectButton.GetComponent<Image> ().color = Color.yellow;
		UpdateObjectMode ();
	}

	public void SwitchToMoveMode()
	{
		objMode = EditorCamController.ObjectMode.Move;
		DeselectAllButtons ();
		moveButton.GetComponent<Image> ().color = Color.yellow;
		UpdateObjectMode ();
	}

	public void SwitchToRotateMode()
	{
		objMode = EditorCamController.ObjectMode.Rotate;
		DeselectAllButtons ();
		rotateButton.GetComponent<Image> ().color = Color.yellow;
		UpdateObjectMode ();
	}

	void UpdateObjectMode()
	{
		GameObject selectedObj = editorController.GetSelectedObject ();
		if (selectedObj != null) {
			if (selectedObj.name != "terrain")
				selectedObj.GetComponent<ObjectManipulator> ().objMode = objMode;
		}
	}

	void DeselectAllButtons ()
	{
		moveButton.GetComponent<Image> ().color = Color.white;
		rotateButton.GetComponent<Image> ().color = Color.white;
		selectButton.GetComponent<Image> ().color = Color.white;
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

	public void SwitchToLogic()
	{
//		GameObject selectedObj = editorController.GetSelectedObject ();
//		Debug.Log ("selected: " + selectedObj.name);
		MMVR_Core.Instance.ToggleLogic ();
	}
}

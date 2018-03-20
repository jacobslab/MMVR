using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectedObjectController : MonoBehaviour {

	EditorCamController editorController { get { return EditorCamController.Instance; }}
	public enum ObjectMode {
		Select,
		Move
	}
	public ObjectMode objMode;
	public Button selectButton;
	public Button moveButton;
	// Use this for initialization
	void Start () {
		selectButton.Select ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SwitchToSelectMode()
	{
		objMode = ObjectMode.Select;

		moveButton.transform.GetChild (1).GetComponent<Image> ().color = Color.white;
		selectButton.transform.GetChild (1).GetComponent<Image> ().color = Color.yellow;
	}

	public void SwitchToMoveMode()
	{
		objMode = ObjectMode.Move;
		selectButton.transform.GetChild (1).GetComponent<Image> ().color = Color.white;
		moveButton.transform.GetChild (1).GetComponent<Image> ().color = Color.yellow;
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
		GameObject selectedObj = editorController.GetSelectedObject ();
		Debug.Log ("selected: " + selectedObj.name);
		MMVR_Core.Instance.ToggleLogic (selectedObj);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;

public class VariablePanel : InteractableUIElement {

	public enum VariableType
	{
		Integer,
		Float,
		Vector3,
		Bool
	}
	public VariableType varType;
	public Transform canvasTransform;
	public Transform originalParent;
	public Vector3 originalAnchoredPos3D;
	private Vector3 lastDraggedPos;

	public Vector3 fixedBoxPos;

	public GameObject varPlaygroundRef;
	public List<GameObject> variableBoxes;

	public List<GameObject> spawnedVarBoxes;
	GameObject varBox;
	public Dropdown varTypeDropdown;
	public Text objName;
	// Use this for initialization
	void Start () {
		canvasTransform = GetComponentInParent<Canvas> ().transform;
		originalParent = transform.parent;
		originalAnchoredPos3D = GetComponent<RectTransform> ().anchoredPosition3D;
		SetupPanel ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void ChangeVariableType()
	{
		switch (varTypeDropdown.value) {
		case 0:
			varType = VariableType.Integer;
			break;
		case 1:
			varType = VariableType.Vector3;
			break;
		case 2:
			varType = VariableType.Float;
			break;
		case 3:
			varType = VariableType.Bool;
			break;
		}

		DropSpawnObject ();

	}

	//use this to setup the place panel
	public void SetupPanel()
	{
		DropSpawnObject ();
//		objName.text = name;
		//		previewImage.texture = AssetPreview.GetAssetPreview (gameObj);
	}
//
//	public override void OnPointerEnter(PointerEventData eventData)
//	{
//		transform.SetParent (canvasTransform, true);
//	}
//	void DropBackToPanel()
//	{
//		transform.SetParent(originalParent,true);
//		GetComponent<RectTransform> ().anchoredPosition3D = originalAnchoredPos3D;
//	}
//
//	public override void OnBeginDrag(PointerEventData data)
//	{
//		selected = true;
//		Debug.Log("They started dragging " + this.name);
//	}
//
//	public override void OnDrag(PointerEventData data)
//	{
//		GetComponent<RectTransform>().anchoredPosition3D = Camera.main.ScreenToViewportPoint (GetMousePosInWorldCoords());
//		GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (GetComponent<RectTransform> ().anchoredPosition3D.x * Screen.width, GetComponent<RectTransform> ().anchoredPosition3D.y * Screen.height);
//		lastDraggedPos = Camera.main.ScreenToWorldPoint (GetMousePosInWorldCoords());
//	}
//
//	public override void OnEndDrag(PointerEventData data)
//	{
//		DropSpawnObject ();
//		Debug.Log ("deselected:" + gameObject.name);
//		selected = false;
//		DropBackToPanel ();
//	}
//
	void DropSpawnObject()
	{

		if (varBox != null) {
			spawnedVarBoxes.Remove (varBox);
			Debug.Log ("destroyed old var box");
			Destroy (varBox);
		}

		switch (varType) {
		case VariableType.Integer:
			Debug.Log ("drop spawned integer");
			varBox = Instantiate (variableBoxes [0], Vector3.zero, Quaternion.identity) as GameObject;
			break;
		case VariableType.Vector3:
			Debug.Log ("drop spawned Vector3");
			varBox = Instantiate (variableBoxes [1], lastDraggedPos, Quaternion.identity) as GameObject;
			break;
		case VariableType.Float:
			Debug.Log ("drop spawned Float");
			varBox = Instantiate (variableBoxes [2], lastDraggedPos, Quaternion.identity) as GameObject;
			break;
		case VariableType.Bool:
			Debug.Log ("drop spawned Bool");
			varBox = Instantiate (variableBoxes [3], lastDraggedPos, Quaternion.identity) as GameObject;
			break;

		}
		if (varBox != null) {
			varBox.transform.parent = varPlaygroundRef.transform;
			varBox.GetComponent<RectTransform> ().anchoredPosition3D = fixedBoxPos;
			spawnedVarBoxes.Add (varBox);
		}
	}
}

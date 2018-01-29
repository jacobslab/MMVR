using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UtilityBox : InteractableUIElement {
	public Text utilityNameText;
	public Transform canvasTransform;

	public List<Object> arguments;
	public enum InputDataType
	{
		Str,
		Int
	}
	public enum UtilityType
	{
		PrintString

	}

	// Use this for initialization
	void Start () {
		canvasTransform = GameObject.FindGameObjectWithTag("Canvas").transform;
		arguments = new List<Object> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetupUtilityBox(string name,UtilityType utilityType)
	{
		utilityNameText.text = name;
		switch (utilityType) {
		case UtilityType.PrintString:
			//add input
			AddInputs(1,"to be printed",InputDataType.Str);
			//add function
			break;
		}
	}

	void AddInputs (int numInputs, string inputNmae, InputDataType inputDataType)
	{
//		for (int i = 0; i < numInputs; i++) {
//			switch (inputDataType[i]) {
//			case InputDataType.Str:
//				arguments.Add ((Object)inputNames[i]);
//				break;
//			}
//		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		transform.SetParent (canvasTransform, true);
	}
	public override void OnBeginDrag(PointerEventData data)
	{
		Debug.Log("They started dragging " + this.name);
	}
	public override void OnEndDrag(PointerEventData data)
	{
		Debug.Log("Stopped dragging " + this.name);
	}
	public override void OnDrag(PointerEventData data)
	{
		GetComponent<RectTransform>().anchoredPosition3D = Camera.main.ScreenToViewportPoint (GetMousePosInWorldCoords());
		GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (GetComponent<RectTransform> ().anchoredPosition3D.x * Screen.width, GetComponent<RectTransform> ().anchoredPosition3D.y * Screen.height);
		//		lastDraggedPos = Camera.main.ScreenToWorldPoint (GetMousePosInWorldCoords());
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UtilityBox : InteractableUIElement {
	public Text utilityNameText;
	public Transform canvasTransform;
	public string displayName;

	public GameObject functionConnectedTo;
	public List<GameObject> arguments;
	public List<Transform> argTransforms;
	public IEnumerator coroutineToBeExecuted;
	public enum InputDataType
	{
		Str,
		Int
	}


	private bool outConnecting=false;
	public GameObject inPin;
	public GameObject outPin;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void AddInputs (int numInputs, string inputName, InputDataType inputDataType)
	{
		switch (inputDataType) {
		case InputDataType.Str:
			for (int i = 0; i < numInputs; i++) {
				GameObject newObj = new GameObject();
				newObj.transform.parent = this.transform;
				newObj.AddComponent<InputField> ();
			}
			break;
		}
//		for (int i = 0; i < numInputs; i++) {
//			switch (inputDataType[i]) {
//			case InputDataType.Str:
//				arguments.Add ((Object)inputNames[i]);
//				break;
//			}
//		}
	}
	public void OutButtonPressed()
	{
		outConnecting = true;
	}

	void CheckForUtilityConnection(GameObject droppedObj)
	{
		GameObject droppedPin;
		if (droppedObj != null) {
			//Debug.Log ("dropped object is : " + droppedObj.name);
			if (droppedObj.name.Contains("InPin")) {
				droppedPin = droppedObj;
				droppedObj = droppedObj.transform.parent.gameObject;
				//Debug.Log ("connected with utilitybox");
				//chain the function you're connected to the utility you're connecting with
				droppedObj.GetComponent<UtilityBox> ().functionConnectedTo = functionConnectedTo;
				//			droppedObj.GetComponent<UtilityBox> ().AddCoroutineTo (this.gameObject);

				//only add if it hasn't already been added before to this function
				bool foundMatch = false;
				for (int i = 0; i < functionConnectedTo.GetComponent<FunctionBox>().utilitiesConnected.Count; i++) {
					if (functionConnectedTo.GetComponent<FunctionBox>().utilitiesConnected [i] == droppedObj.GetComponent<UtilityBox> ())
						foundMatch = true;
				}
				if (!foundMatch) {
					functionConnectedTo.GetComponent<FunctionBox>().utilitiesConnected.Add (droppedObj.GetComponent<UtilityBox> ());
					droppedPin.GetComponent<Image> ().color = Color.gray;
					outPin.GetComponent<Image> ().color = Color.gray;
				}
			}
		}

	}


	public virtual void AddCoroutineTo(GameObject functionAskingToBeConnected)
	{
		functionConnectedTo.GetComponent<FunctionBox> ().AddToSequence (coroutineToBeExecuted);
		//Debug.Log ("added coroutine to the function");
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		transform.SetParent (canvasTransform, true);
	}
	public override void OnBeginDrag(PointerEventData data)
	{
//		Debug.Log("They started dragging " + this.name);
	}
	public override void OnEndDrag(PointerEventData data)
	{
		//Debug.Log("Stopped dragging " + this.name);
		CheckForUtilityConnection (data.pointerEnter);
		if (outConnecting)
			outConnecting = false;
	}
	public override void OnDrag(PointerEventData data)
	{		
		if (data.pointerPress == this.gameObject) {
			GetComponent<RectTransform> ().anchoredPosition3D = Camera.main.ScreenToViewportPoint (GetMousePosInWorldCoords ());
			GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (GetComponent<RectTransform> ().anchoredPosition3D.x * Screen.width, GetComponent<RectTransform> ().anchoredPosition3D.y * Screen.height);
		//		lastDraggedPos = Camera.main.ScreenToWorldPoint (GetMousePosInWorldCoords());
		}
	}
}

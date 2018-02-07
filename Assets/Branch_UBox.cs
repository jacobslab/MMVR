using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Branch_UBox : UtilityBox{

	private GameObject activePin;
	public GameObject truePin;
	public GameObject falsePin;
	public Toggle conditionPin;

	public UtilityBox trueConnectedToUtility;
	public UtilityBox falseConnectedToUtility;

	public override void AddCoroutineTo(GameObject functionAskingToBeConnected)
	{
		//Debug.Log ("added to the function");
		functionConnectedTo.GetComponent<FunctionBox> ().AddToSequence (Branch(conditionPin.isOn));
	}

	public override void CheckForUtilityConnection(GameObject droppedObj)
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
					if(currentlyDraggedPin.name=="TruePin")
						trueConnectedToUtility=droppedObj.GetComponent<UtilityBox> ();
					else
						falseConnectedToUtility=droppedObj.GetComponent<UtilityBox> ();
					
					droppedPin.GetComponent<Image> ().color = Color.gray;
					currentlyDraggedPin.GetComponent<Image> ().color = Color.gray;
				}
			}
		}

	}
	public override void OnBeginDrag(PointerEventData data)
	{
		if (data.pointerPress.name == "TruePin" || data.pointerPress.name == "FalsePin") {
			currentlyDraggedPin = data.pointerPress;
		}
		Debug.Log("Dragged pin " + data.pointerPress.name);
	}
	IEnumerator Branch(bool condition)
	{
		bool foundMatch = false;

		if (condition) {
			activePin = truePin;
			Debug.Log ("IT'S TRUE");
//			for (int i = 0; i < functionConnectedTo.GetComponent<FunctionBox>().utilitiesConnected.Count; i++) {
//				if (functionConnectedTo.GetComponent<FunctionBox>().utilitiesConnected [i] == trueConnectedToUtility)
//					foundMatch = true;
//			}
//			if (!foundMatch) {
			yield return StartCoroutine(trueConnectedToUtility.ExecuteCoroutine());
//			}
		} else {
			Debug.Log ("it's false");
//			for (int i = 0; i < functionConnectedTo.GetComponent<FunctionBox>().utilitiesConnected.Count; i++) {
//				if (functionConnectedTo.GetComponent<FunctionBox>().utilitiesConnected [i] == falseConnectedToUtility)
//					foundMatch = true;
//			}
//			if (!foundMatch) {

			yield return StartCoroutine(falseConnectedToUtility.ExecuteCoroutine());
//			}
			activePin = falsePin;
		}
		yield return null;
	}

}

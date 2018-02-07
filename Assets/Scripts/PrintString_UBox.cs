using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PrintString_UBox : UtilityBox {

	public InputField textInput;

	// Update is called once per frame
	void Update () {
	}

	public override void AddCoroutineTo(GameObject functionAskingToBeConnected)
	{
		functionConnectedTo.GetComponent<FunctionBox> ().AddToSequence (PrintString(textInput.text));
		//Debug.Log ("added to the function");
	}

	public override IEnumerator ExecuteCoroutine()
	{
		yield return StartCoroutine (PrintString (textInput.text));
		yield return null;
	}

	IEnumerator PrintString(string toBePrinted)
	{
		UnityEngine.Debug.Log (toBePrinted);
		yield return null;
	}


}

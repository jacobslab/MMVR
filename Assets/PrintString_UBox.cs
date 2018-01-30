using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PrintString_UBox : UtilityBox {

	public InputField textInput;
	// Use this for initialization
	void Start () {
		argTransforms = new List<Transform> ();
		canvasTransform = GameObject.FindGameObjectWithTag("Canvas").transform;
		arguments = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void AddCoroutineTo(GameObject functionAskingToBeConnected)
	{
		functionConnectedTo.GetComponent<FunctionBox> ().AddToSequence (PrintString(textInput.text));
		Debug.Log ("added to the function");
	}

	IEnumerator PrintString(string toBePrinted)
	{
		UnityEngine.Debug.Log (toBePrinted);
		yield return null;
	}


}

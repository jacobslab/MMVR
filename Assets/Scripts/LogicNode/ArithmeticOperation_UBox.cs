using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArithmeticOperation_UBox : UtilityBox {

	public InputField leftOperand;
	public InputField rightOperand;
	public Dropdown operationSign;
	public Text name;

	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateDescriptorText()
	{
		switch (operationSign.value) {
		case 0:
			name.text = "Add";
			break;
		case 1:
			name.text = "Subtract";
			break;
		case 2:
			name.text = "Multiply";
			break;
		case 3:
			name.text = "Divide";
			break;

		}
	}
	public override void AddCoroutineTo(GameObject functionAskingToBeConnected)
	{
		functionConnectedTo.GetComponent<FunctionBox> ().AddToSequence (PerformOperation(leftOperand,rightOperand,operationSign));
		Debug.Log ("added to the function");
	}
	public override IEnumerator ExecuteCoroutine()
	{
		yield return StartCoroutine (PerformOperation(leftOperand,rightOperand,operationSign));
		yield return null;
	}

	IEnumerator PerformOperation(InputField leftOperand, InputField rightOperand, Dropdown dropdown)
	{
		int result = 0;
		switch (dropdown.value) {
		case 0:
			result = int.Parse (leftOperand.text) + int.Parse (rightOperand.text);
			UnityEngine.Debug.Log (result.ToString ());
			break;
		case 1:
			result = int.Parse (leftOperand.text) - int.Parse (rightOperand.text);
			UnityEngine.Debug.Log (result.ToString ());
			break;
		case 2:
			result = int.Parse (leftOperand.text) * int.Parse (rightOperand.text);
			UnityEngine.Debug.Log (result.ToString ());
			break;
		case 3:
			result = int.Parse (leftOperand.text) / int.Parse (rightOperand.text);
			UnityEngine.Debug.Log (result.ToString ());
			break;
		}

		yield return null;
	}
}

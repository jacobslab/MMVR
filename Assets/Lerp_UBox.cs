using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEditor;
public class Lerp_UBox : UtilityBox {

	public Dropdown startPosArg;
	public Dropdown endPosArg;
	public Dropdown interpolantArg;

	public override void AddCoroutineTo(GameObject functionAskingToBeConnected)
	{
		//Debug.Log ("added to the function");
		functionConnectedTo.GetComponent<FunctionBox> ().AddToSequence (LerpVec3());
	}


	public override IEnumerator ExecuteCoroutine()
	{
		yield return StartCoroutine (LerpVec3 ());
		yield return null;
	}

	IEnumerator LerpVec3()
	{
		yield return null;
	}
}

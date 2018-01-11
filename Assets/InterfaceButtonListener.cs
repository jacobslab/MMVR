using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
public class InterfaceButtonListener: MonoBehaviour
{
	public InterfaceButton interfaceButton;
	public List<IEnumerator> coroutineSequence;
	void Start()
	{
		coroutineSequence = new List<IEnumerator> ();
		
	}

	void Update()
	{
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				GameObject hitObj = hit.collider.gameObject;
				Debug.Log ("hit " + hitObj.name.ToString ());
				if (hitObj.GetComponent<InterfaceButton> () != null) {
					hit.collider.gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
					interfaceButton = hit.collider.gameObject.GetComponent<InterfaceButton> ();
				}
			}
		

		}
	}

	//execute actions

	public void ExecuteAssignedActions()
	{
		if (interfaceButton != null) {
			StartCoroutine (ExecuteCoroutines ());
			//StartCoroutine(interfaceButton.ExecuteAction ());
		}
	}

	IEnumerator ExecuteCoroutines()
	{
		for (int i = 0; i < coroutineSequence.Count; i++) {
			yield return StartCoroutine (interfaceButton.ExecuteAction (coroutineSequence[i]));
		}
		yield return null;
	}

	//randomize color
	public void AddRandomizeColorToEvent()
	{
		Debug.Log ("about to scale");
		coroutineSequence.Add (StartRandomizeColorEvent(interfaceButton));
//		interfaceButton.ButtonPressedCoroutine += StartRandomizeColorEvent;
	}

	IEnumerator StartRandomizeColorEvent(InterfaceButton ib)
	{
		Debug.Log ("scaling object");
		yield return StartCoroutine(RandomizeColor(ib));
		yield return null;
	}

	IEnumerator RandomizeColor(InterfaceButton ib)
	{
//		Color col = new Color (UnityEngine.Random.Range (0f, 1f), UnityEngine.Random.Range (0f, 1f), UnityEngine.Random.Range (0f, 1f));
		ib.transform.localScale = new Vector3 (UnityEngine.Random.Range (0.1f, 3f), UnityEngine.Random.Range (0.1f, 3f), UnityEngine.Random.Range (0.1f, 3f));
		yield return null;
	}

	//move object

	public void AddMoveObject()
	{
		Debug.Log ("about to move");
		coroutineSequence.Add (StartMoveObjectCoroutine(interfaceButton));
		//interfaceButton.ButtonPressedCoroutine += StartMoveObjectCoroutine;

	}

	IEnumerator StartMoveObjectCoroutine(InterfaceButton ib)
	{

		Debug.Log ("moving object");
		//Debug.Log ("handling " + ib.gameObject.name.ToString ());
		yield return StartCoroutine(MoveObject(ib));
	}

	IEnumerator MoveObject(InterfaceButton ib)
	{
		float factor = 0f;
		Vector3 destPos = ib.transform.position + new Vector3 (7f, 4f, 0f);
		while (factor < 3f) {
			factor += Time.deltaTime;
				ib.transform.position = Vector3.Lerp (ib.transform.position,destPos, factor / 3f);
			yield return 0;
		}
		yield return null;
		yield return null;

	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class FunctionBox : InteractableUIElement {
	public Text funcNameText;
	private List<IEnumerator> activeSequence;
	public Transform canvasTransform;

	// Use this for initialization
	void Start () {
		canvasTransform = GameObject.FindGameObjectWithTag("Canvas").transform;
		activeSequence = new List<IEnumerator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			Debug.Log ("added OK");
			AddToSequence (PrintCoroutine("OK"));
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			Debug.Log ("added THEN");
			AddToSequence (PrintCoroutine ("THEN"));
		}
		if (Input.GetKeyDown (KeyCode.Return)) {
			ExecuteSequence ();
		}
	}

	public void SetupFunctionBox(string name)
	{
		funcNameText.text = name; 
	}

	void ExecuteSequence()
	{
		Debug.Log ("executing sequence");
		IEnumerator[] seqArr = new IEnumerator[activeSequence.Count];
		for (int i = 0; i < activeSequence.Count; i++) {
			seqArr [i] = activeSequence [i];
		}
//		activeSequence.Clear ();
		Debug.Log(seqArr[0]);
		StartCoroutine(Sequence (seqArr));
	}
	public static IEnumerator Sequence(params IEnumerator[] sequence)
	{
		Debug.Log ("inside sequence " + " with length " + sequence.Length.ToString());
		for(int i = 0 ; i < sequence.Length; ++i)
		{
			while(sequence[i].MoveNext())
				yield return sequence[i].Current;
		}
	}


	IEnumerator PrintCoroutine(string arg)
	{
		Debug.Log (arg);
		yield return new WaitForSeconds(0.3f);
	}

	void AddToSequence(IEnumerator coroutineToBeAdded)
	{
		activeSequence.Add (coroutineToBeAdded);
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

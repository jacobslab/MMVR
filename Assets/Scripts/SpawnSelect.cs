using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SpawnSelect : InteractableUIElement {

	public GameObject associatedObj;
	public int assignedIndex=0;
	private Text textComp;
	// Use this for initialization
	void Start () {
		textComp = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnPointerEnter( PointerEventData eventData )
	{
	}
	public override void OnPointerDown( PointerEventData eventData )
	{
//		Debug.Log ("click the text");
		SelectObject();

		//make the selected index of hierarchy equal to your assigned index
		ObjectPanelManager.selectedIndex = assignedIndex;
	}

	public void DestroyObject()
	{
		
	}

	public void AdjustPosition()
	{
		GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (30f,150f + (assignedIndex * -20f), 0f);
	}

	public IEnumerator DestroyTextObject()
	{
		Destroy (gameObject);
		yield return null;
	}



	public override void SelectObject()
	{
		textComp.color = Color.red;
		selected = true;
		EditorCamController.Instance.SetSelectedObject (associatedObj);

	}

	public override void DeselectObject()
	{
		textComp.color = Color.black;
		selected = false;
	}

}

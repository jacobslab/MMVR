using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SpawnSelect : InteractableUIElement {

	public Button hierarchyButton;
	public GameObject associatedObj;
	public int assignedIndex=0;
	private Text textComp;

	public Color normalColor;
	public Color selectedColor;
	// Use this for initialization
	void Start () {
		textComp = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void DestroyObject()
	{
		
	}
	public void OnSelectButton()
	{
		Debug.Log ("click the text");
		SelectObject();

		//make the selected index of hierarchy equal to your assigned index
		ObjectPanelManager.selectedIndex = assignedIndex;
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



	public void Select()
	{
		textComp.color = Color.red;
		selected = true;
		hierarchyButton.image.color = selectedColor;
		EditorCamController.Instance.SetSelectedObject (associatedObj);

	}

	public void Deselect()
	{
		Debug.Log ("deselected");
		textComp.color = Color.black;
		hierarchyButton.image.color = normalColor;
		selected = false;
	}

}

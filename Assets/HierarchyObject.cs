using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HierarchyObject : MonoBehaviour {

	public GameObject hierarchyButton;
	public GameObject hierarchyInput;
	public Text hierarchyText;
	public SpawnSelect spawnSelect;

	private bool isSelected=false;

	// Use this for initialization
	void Awake () {
		//hierarchyText = transform.GetChild (0).GetComponent<Text> ();
		SwitchToButton ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeName(string newName)
	{
		hierarchyText.text = newName;
	}

	public void UpdateName()
	{
		ChangeName (hierarchyInput.GetComponent<InputField> ().text);
		SwitchToButton ();
	}

	void SwitchToButton()
	{
		hierarchyButton.SetActive (true);
		hierarchyInput.SetActive (false);
	}

	void SwitchToInputField()
	{
		hierarchyButton.SetActive (false);
		hierarchyInput.SetActive (true);
	}
	public void SelectObject()
	{
		//if selected, then it's being clicked again --> change name
		if (isSelected) {
			SwitchToInputField ();
		} else {
			Debug.Log ("button clicked");
			HierarchyManager.Instance.UpdateSelection (gameObject.GetComponent<HierarchyObject> ());
			spawnSelect.Select ();
			isSelected = true;
		}
	}
	public void DeselectObject()
	{
		Debug.Log ("deselected");
		isSelected = false;
		spawnSelect.Deselect ();
	}
}

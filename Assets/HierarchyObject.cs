using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HierarchyObject : MonoBehaviour {

	private Text hierarchyText;
	public SpawnSelect spawnSelect;

	// Use this for initialization
	void Awake () {
		hierarchyText = transform.GetChild (0).GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeName(string newName)
	{
		hierarchyText.text = newName;
	}
	public void SelectObject()
	{
		Debug.Log ("button clicked");
		HierarchyManager.Instance.UpdateSelection (gameObject.GetComponent<HierarchyObject>());
		spawnSelect.Select ();
	}
	public void DeselectObject()
	{
		Debug.Log ("deselected");
		spawnSelect.Deselect ();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPanelManager : MonoBehaviour {

	public GameObject textPrefab;
	private int index=0;
	public static int selectedIndex=0;
	// Use this for initialization
	void OnEnable () {

//		HierarchyManager.textSpawnedEvent += AddTextObject(this);
	}

	void OnDisable()
	{

//		HierarchyManager.textSpawnedEvent -= AddTextObject(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void IncrementIndex()
	{
		index++;
	}

	public void DecrementIndex()
	{
		index--;
	}

	public GameObject AddTextObject(string text,SpawnableObject obj)
	{
		//Debug.Log ("spawned text");
		GameObject newText = Instantiate (textPrefab);
		newText.GetComponent<Text> ().text = text;
		newText.transform.parent = this.transform;
		newText.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (30f,150f + (index * -90f), 0f);
		newText.GetComponent<SpawnSelect> ().associatedObj = obj.gameObj;
		newText.GetComponent<SpawnSelect> ().assignedIndex = index;

		//increment the index then
		IncrementIndex();

		return newText;

	}
}

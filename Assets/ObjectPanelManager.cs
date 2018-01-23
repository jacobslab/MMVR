using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPanelManager : MonoBehaviour {

	public GameObject textPrefab;
	private int index=0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddTextObject(string text,SpawnableObject obj)
	{
		//Debug.Log ("spawned text");
		GameObject newText = Instantiate (textPrefab);
		newText.GetComponent<Text> ().text = text;
		newText.transform.parent = this.transform;
		newText.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (30f,150f + (index * -20f), 0f);
		newText.GetComponent<SpawnSelect> ().associatedObj = obj.ok;
		index++;
	}
}

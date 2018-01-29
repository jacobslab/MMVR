using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour {

	public GameObject placeablePanel;
	public GameObject placePanelParent;
	// Use this for initialization
	void Awake () {
		PopulatePlacePanel ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//loaded once at the start of the editor level
	void PopulatePlacePanel()
	{
		GameObject[] placeableObjArr = Resources.LoadAll<GameObject> ("Placeables");
		Sprite[] placeableSpriteArr = Resources.LoadAll<Sprite> ("PlaceableImages");
		Debug.Log ("count:" + placeableObjArr.Length.ToString ());
		for (int i = 0; i < placeableObjArr.Length; i++) {
			GameObject newPlacePanel = Instantiate (placeablePanel, Vector3.zero, Quaternion.identity) as GameObject;
			newPlacePanel.transform.parent = placePanelParent.transform;
			newPlacePanel.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (-39.1f,(i * -90f), 0f);
			newPlacePanel.GetComponent<PlacePanel> ().SetupPanel (placeableObjArr [i].gameObject.name, placeableSpriteArr [i]);
			newPlacePanel.GetComponent<PlacePanel> ().objectType = placeableObjArr [i].GetComponent<ObjectManipulator> ().objType;
		}
	}
}

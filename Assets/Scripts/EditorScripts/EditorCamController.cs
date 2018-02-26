using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public class EditorCamController : MonoBehaviour {
	GameObject selectedObj;
	OrbitMouse mouseOrbit;

	public PlaceManager placeManager;
	public ObjectPanelManager panelManager;
	public HierarchyManager hierarchyManager;

	private string json;
	private EventManager eventManager;
	public List<string> spawnToJson;
	//EXPERIMENT IS A SINGLETON
	private static EditorCamController _instance;
	public static EditorCamController Instance{
		get{
			return _instance;
		}
	}

	void Awake(){
		if (_instance != null) {
			Debug.Log ("Instance already exists!");
			return;
		}
		_instance = this;
		mouseOrbit = GetComponent<OrbitMouse> ();

		//event handler associations 
		EventManager.OnDestroyed += Destroy;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (selectedObj != null) {
			if (Input.GetKeyDown (KeyCode.Backspace)) {
				StartCoroutine("DestroyObject");
			}
		}
	}

	public void SetSelectedObject(GameObject objSelected)
	{
		selectedObj = objSelected;
		string cleanedName = Regex.Replace(objSelected.name, @"[\d-]", string.Empty);
		cleanedName = Regex.Replace (cleanedName, "[_]", string.Empty);

		placeManager.propertyManager.SwitchToPanel (objSelected.name);
		if (selectedObj != null)
			mouseOrbit.target = selectedObj.transform;
	}
		

	public GameObject GetSelectedObject()
	{
		return selectedObj;
	}
	public void Load()
	{
		for (int i = 0; i < spawnToJson.Count; i++) {
			SpawnableObject respawnedObj= JsonUtility.FromJson<SpawnableObject> (spawnToJson[i]);
			if (hierarchyManager.spawnedObjList [i].gameObj == null) {
				switch (respawnedObj.objType) {
				case SpawnableObject.ObjectType.Cube:
					//Debug.Log ("respawning cube");
					placeManager.CreateCube(respawnedObj.objName,respawnedObj.pos, respawnedObj.rot);
					break;
				case SpawnableObject.ObjectType.Character:
					//Debug.Log ("respawning character");
					placeManager.CreateCharacter(respawnedObj.objName,respawnedObj.pos, respawnedObj.rot);
					break;

				}
				ClearNullObjects ();
			}
			//Debug.Log ("respawned objects" + respawnedObj.pos.ToString());
		}
	}
	public void Save()
	{
		spawnToJson.Clear ();

		ClearNullObjects ();
		Debug.Log("spawn to json list length is: " + spawnToJson.Count.ToString());
		Debug.Log ("spawnobjlist length is: " + hierarchyManager.spawnedObjList.Count.ToString());	
		for(int i=0;i<hierarchyManager.spawnedObjList.Count;i++)
		{
				//Debug.Log ("position on save: " + placeManager.spawnedObjList [i].ok.transform.position.ToString ());
				hierarchyManager.spawnedObjList [i].UpdateValues ();
				spawnToJson.Add (JsonUtility.ToJson (hierarchyManager.spawnedObjList [i], true));
				//Debug.Log ("spawntojson is:" + spawnToJson [i]);
		}
	}

	void ClearNullObjects()
	{
		for(int i=0;i<hierarchyManager.spawnedObjList.Count;i++)
		{
			if (hierarchyManager.spawnedObjList [i].gameObj == null) {
				Debug.Log ("removed some NULL objects at index " + i.ToString());
				hierarchyManager.spawnedObjList.RemoveAt (i);
			}

		}
	}

	IEnumerator DestroyObject()
	{
		if (selectedObj != null) {

			List<GameObject> keyList = new List<GameObject> (hierarchyManager.textSpawnableDict.Keys);
			//search all keys for the selectedobj
			for (int i = 0; i < keyList.Count; i++) {
				SpawnableObject possibleObj;
				hierarchyManager.textSpawnableDict.TryGetValue (keyList[i], out possibleObj);
				if (possibleObj.gameObj == selectedObj) {
					Debug.Log ("found a match");
					yield return StartCoroutine(keyList [i].GetComponent<SpawnSelect> ().DestroyTextObject ());
					//if found a match,then remove that key as we are destroying it
					hierarchyManager.textSpawnableDict.Remove (keyList[i]);
					//decrement the overall index
					panelManager.DecrementIndex ();
					//push up all the indices located below the about to be destroyed index
					hierarchyManager.AdjustTextIndex(i);
					//clear null objects from the list
					ClearNullObjects();
					Destroy ();

				}
			}
		}
		yield return null;
	}

	public void BeginDestroyProcess()
	{
		StartCoroutine("DestroyObject");
	}

	public void Destroy()
	{
		
		if (selectedObj != null) {
			Destroy (selectedObj);
		}
	}


}

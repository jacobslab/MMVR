using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EditorCamController : MonoBehaviour {
	GameObject selectedObj;
	OrbitMouse mouseOrbit;
	public PlaceManager placeManager;
	private string json;

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
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetSelectedObject(GameObject objSelected)
	{
		selectedObj = objSelected;
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
			if (placeManager.spawnedObjList [i].ok == null) {
				switch (respawnedObj.objType) {
				case SpawnableObject.ObjectType.Cube:
					SpawnableObject newObj = new SpawnableObject (respawnedObj.objName, respawnedObj.pos,SpawnableObject.ObjectType.Cube);
					newObj.ok.GetComponent<Renderer> ().material.color = Color.red;
					placeManager.spawnedObjList.Add (newObj);
					break;

				}
			}
			Debug.Log ("respawned objects" + respawnedObj.pos.ToString());
		}
	}
	public void Save()
	{
		spawnToJson.Clear ();
		ClearNullObjects();
		Debug.Log("spawn to json list length is: " + spawnToJson.Count.ToString());
		Debug.Log ("spawnobjlist length is: " + placeManager.spawnedObjList.Count.ToString());	
		for(int i=0;i<placeManager.spawnedObjList.Count;i++)
		{
				Debug.Log ("position on save: " + placeManager.spawnedObjList [i].ok.transform.position.ToString ());
				placeManager.spawnedObjList [i].UpdateValues ();
				spawnToJson.Add (JsonUtility.ToJson (placeManager.spawnedObjList [i], true));
				Debug.Log ("spawntojson is:" + spawnToJson [i]);
		}
	}

	void ClearNullObjects()
	{
		for(int i=0;i<placeManager.spawnedObjList.Count;i++)
		{
		if (placeManager.spawnedObjList [i].ok == null)
			placeManager.spawnedObjList.RemoveAt (i);
		}
		Debug.Log ("removed any null objects");
	}

	public void Destroy()
	{
		
		if (selectedObj != null) {
			Destroy (selectedObj);
		}
	}


}

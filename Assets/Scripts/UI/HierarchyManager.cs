using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HierarchyManager : MonoBehaviour {

	public delegate void TextSpawnedEventHandler(object sender);
	public static event TextSpawnedEventHandler textSpawnedEvent;
	public Dictionary<GameObject,SpawnableObject> textSpawnableDict;
	public List<GameObject> spawnSelectTextObj;
	public List<SpawnableObject> spawnedObjList;

	public HierarchyObject selectedHierarchyObject;

	//singleton
	private static HierarchyManager _instance;
	public static HierarchyManager Instance{
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
	}
	// Use this for initialization
	void Start () {
		textSpawnableDict = new Dictionary<GameObject, SpawnableObject>();
		spawnedObjList = new List<SpawnableObject> ();
		spawnSelectTextObj = new List<GameObject> ();
		EventManager.CompleteSetup ();
	}

	public void AddDictEntry(GameObject textObj, SpawnableObject spawnObj)
	{
		textSpawnableDict.Add (textObj, spawnObj);
	}

	public void RemoveDictEntry(GameObject textObj)
	{
		textSpawnableDict.Remove (textObj);
	}

	public void AdjustTextIndex(int index)
	{
		List<GameObject> keyList = new List<GameObject> (textSpawnableDict.Keys);
		for (int i = index; i < keyList.Count; i++) {
			keyList [i].GetComponent<SpawnSelect> ().assignedIndex -= 1;
			keyList [i].GetComponent<SpawnSelect> ().AdjustPosition ();
		}
	}
	public void UpdateSelection(HierarchyObject newObj)
	{
		if(selectedHierarchyObject!=null)
			selectedHierarchyObject.DeselectObject ();
		
		selectedHierarchyObject = newObj;
	}
	// Update is called once per frame
	void Update () {
		
	}
}

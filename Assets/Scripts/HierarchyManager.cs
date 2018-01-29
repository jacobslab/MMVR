using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HierarchyManager : MonoBehaviour {

	public delegate void TextSpawnedEventHandler(object sender);
	public static event TextSpawnedEventHandler textSpawnedEvent;
	public Dictionary<GameObject,SpawnableObject> textSpawnableDict;
	public List<GameObject> spawnSelectTextObj;
	public List<SpawnableObject> spawnedObjList;
	// Use this for initialization
	void Start () {
		textSpawnableDict = new Dictionary<GameObject, SpawnableObject>();
		spawnedObjList = new List<SpawnableObject> ();
		spawnSelectTextObj = new List<GameObject> ();
		
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
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public class EditorCamController : MonoBehaviour {
	GameObject selectedObj;

	public PlaceManager placeManager;
	public ObjectPanelManager panelManager;
	public HierarchyManager hierarchyManager;
	public float moveFactor=5f;
	public float rotFactorH=2f;
	public float rotFactorV=2f;
	private float yaw,pitch=0f;
	private string json;
	private EventManager eventManager;
	public List<string> spawnToJson;


	public enum ObjectMode {
		Select,
		Move,
		Rotate
	}
	public ObjectMode objMode;

	public GameObject transformHandlePrefab;
	private GameObject transformHandleObj;

	public bool objSelected=false;

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

		//event handler associations 
		EventManager.OnDestroyed += Destroy;
	}
	// Use this for initialization
	void Start () {
		transformHandleObj = Instantiate (transformHandlePrefab, Vector3.zero, Quaternion.identity) as GameObject;

	}
	
	// Update is called once per frame
	void Update () {
		if (selectedObj != null) {
			if (Input.GetKeyDown (KeyCode.Backspace)) {
				StartCoroutine("DestroyObject");
			}
		}
		if (Input.GetMouseButton(1)) {

			yaw += rotFactorH * Input.GetAxis ("Mouse X");
			pitch -= rotFactorV * Input.GetAxis ("Mouse Y");

			RotateCamera ();

			if (Input.GetKey (KeyCode.W))
				MoveCamera (0,1);
			else if (Input.GetKey (KeyCode.A))
				MoveCamera (-1,0);
			else if (Input.GetKey (KeyCode.S))
				MoveCamera (0,-1);
			else if (Input.GetKey (KeyCode.D))
				MoveCamera (1,0);
		}
	}

	void MoveCamera(int hAxis, int vAxis)
	{
		if (hAxis==1) {
			transform.position += transform.right * moveFactor;
		} 
		else if (hAxis==-1) {
			transform.position += transform.right * -moveFactor;
		} 
		else if (vAxis==1) {
			transform.position += transform.forward * moveFactor;
		}else if (vAxis == -1) {
			transform.position += transform.forward * -moveFactor;
		}

	}

	void RotateCamera()
	{
//		if (mouseXDel > 0f)
//			transform.eulerAngles += Vector3.forward * rotFactor;
//		else if (mouseXDel < 0f)
//			transform.eulerAngles += Vector3.forward * -rotFactor;
		if(!objSelected)
			transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
		
	}

	public void SetSelectedObject(GameObject objToBeSelected)
	{
		Debug.Log ("about to set selected obj: " + objToBeSelected.name);
		if (objToBeSelected != null) {
			Debug.Log ("selected obj is: " + objToBeSelected.name);
			//if another object was selected earlier, deselect that first
			if(selectedObj!=null)
				selectedObj.GetComponent<ObjectManipulator> ().selected = false;
			selectedObj = objToBeSelected;

			//set debug
			DebugUI.Instance.selObjText.text = selectedObj.name;

			string cleanedName = Regex.Replace (objToBeSelected.name, @"[\d-]", string.Empty);
			cleanedName = Regex.Replace (cleanedName, "[_]", string.Empty);

//		SetTransformHandles (objSelected);
			placeManager.propertyManager.SwitchToPanel (objToBeSelected.name);
			MMVR_Core.Instance.logicManager.SwitchToLogicLayer (objToBeSelected.name);

			selectedObj.GetComponent<ObjectManipulator> ().selected = true;
		}

//		if (selectedObj != null)
//			mouseOrbit.target = selectedObj.transform;
	}

	void SetTransformHandles(GameObject objSelected)
	{
		transformHandleObj.transform.parent = objSelected.transform;
		transformHandleObj.transform.localPosition = Vector3.zero;
		transformHandleObj.transform.localRotation = Quaternion.identity;
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

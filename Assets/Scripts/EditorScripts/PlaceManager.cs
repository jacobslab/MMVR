using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlaceManager : MonoBehaviour {

	public GameObject terrain;
	public GameObject cubePrefab;
	public GameObject charPrefab;
	[SerializeField]
	public ObjectPanelManager objPanelManager;
	public HierarchyManager hierarchyManager;
	public PropertyPanelManager propertyManager;

	private int cubeIndex=0;
	private int charIndex=0;
	public GameObject terrainPrefab;
	public Terrain terrainData;
	public Texture2D[] textureArr;
	//EXPERIMENT IS A SINGLETON
	private static PlaceManager _instance;
	public static PlaceManager Instance{
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
		EventManager.OnInitialSetupComplete+=CreateTerrain;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateTerrain()
	{
		SpawnableObject newObj = new SpawnableObject ("terrain", Vector3.zero,Vector3.zero,SpawnableObject.ObjectType.Terrain);

		terrainData = newObj.gameObj.GetComponent<Terrain> ();
		hierarchyManager.spawnedObjList.Add (newObj);
		GameObject newText = objPanelManager.AddTextObject ("terrain",newObj);
		hierarchyManager.AddDictEntry (newText, newObj);
		propertyManager.AddPropertyPanel (SpawnableObject.ObjectType.Terrain,newObj.gameObj);

	}

	public void CreateCube(Vector3 pos)
	{
		string name = "cube_" + cubeIndex.ToString ();
		SpawnableObject newObj = new SpawnableObject (name, pos,Vector3.zero,SpawnableObject.ObjectType.Cube);
		newObj.gameObj.GetComponent<Renderer> ().material.color = Color.red;
		//GameObject newCube = Instantiate (cubePrefab, Camera.main.transform.position + Camera.main.transform.forward * 10f, Quaternion.identity) as GameObject;
		//newCube.GetComponent<Renderer> ().material.color = Color.red;
		hierarchyManager.spawnedObjList.Add (newObj);
		GameObject newText = objPanelManager.AddTextObject (name,newObj);
		hierarchyManager.AddDictEntry (newText, newObj);
		cubeIndex++;
	}

	public void CreateCube(string name, Vector3 pos, Vector3 rot)
	{
		SpawnableObject newObj = new SpawnableObject (name, pos, rot ,SpawnableObject.ObjectType.Cube);
		newObj.gameObj.GetComponent<Renderer> ().material.color = Color.red;
		//GameObject newCube = Instantiate (cubePrefab, Camera.main.transform.position + Camera.main.transform.forward * 10f, Quaternion.identity) as GameObject;
		//newCube.GetComponent<Renderer> ().material.color = Color.red;
		hierarchyManager.spawnedObjList.Add (newObj);
		GameObject newText = objPanelManager.AddTextObject (name,newObj);
		hierarchyManager.AddDictEntry (newText, newObj);
		cubeIndex++;
	}

	public void CreateCharacter(Vector3 pos)
	{
		string name = "character_" + charIndex.ToString ();
		SpawnableObject newObj = new SpawnableObject (name, pos,Vector3.zero,SpawnableObject.ObjectType.Character);
		newObj.gameObj.GetComponent<Renderer> ().material.color = Color.green;
		hierarchyManager.spawnedObjList.Add (newObj);
		GameObject newText = objPanelManager.AddTextObject (name,newObj);
		hierarchyManager.AddDictEntry (newText, newObj);
		charIndex++;
	}
	public void CreateCharacter(string name, Vector3 pos, Vector3 rot)
	{
		SpawnableObject newObj = new SpawnableObject (name,pos, rot ,SpawnableObject.ObjectType.Character);
		newObj.gameObj.GetComponent<Renderer> ().material.color = Color.green;
		hierarchyManager.spawnedObjList.Add (newObj);
		GameObject newText = objPanelManager.AddTextObject (name,newObj);
		hierarchyManager.AddDictEntry (newText, newObj);
		charIndex++;
	}

}

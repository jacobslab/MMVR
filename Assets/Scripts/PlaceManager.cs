using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlaceManager : MonoBehaviour {

	public GameObject cubePrefab;
	public GameObject charPrefab;
	[SerializeField]
	public List<SpawnableObject> spawnedObjList;
	public ObjectPanelManager objPanelManager;

	private int cubeIndex=0;
	private int charIndex=0;

	// Use this for initialization
	void Start () {
		spawnedObjList = new List<SpawnableObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateTerrain()
	{
		
		TerrainData newTerrainData = new TerrainData ();
		GameObject newTerrain = new GameObject ();
		newTerrain = Terrain.CreateTerrainGameObject (newTerrainData);
		GameObject instObj = Instantiate (newTerrain, Vector3.zero, Quaternion.identity) as GameObject;
		//objPanelManager.AddTextObject (instObj.name, instObj);
		//spawnedObjList.Add (instObj);
	}

	public void CreateCube()
	{
		string name = "cube_" + cubeIndex.ToString ();
		SpawnableObject newObj = new SpawnableObject (name, Camera.main.transform.position + Camera.main.transform.forward * 10f,Vector3.zero,SpawnableObject.ObjectType.Cube);
		newObj.ok.GetComponent<Renderer> ().material.color = Color.red;
		//GameObject newCube = Instantiate (cubePrefab, Camera.main.transform.position + Camera.main.transform.forward * 10f, Quaternion.identity) as GameObject;
		//newCube.GetComponent<Renderer> ().material.color = Color.red;
		spawnedObjList.Add (newObj);
		objPanelManager.AddTextObject (name,newObj);
		cubeIndex++;
	}

	public void CreateCharacter()
	{
		string name = "character_" + charIndex.ToString ();
		SpawnableObject newObj = new SpawnableObject (name, Camera.main.transform.position + Camera.main.transform.forward * 10f,Vector3.zero,SpawnableObject.ObjectType.Character);
		newObj.ok.GetComponent<Renderer> ().material.color = Color.green;
		spawnedObjList.Add (newObj);
		objPanelManager.AddTextObject (name,newObj);
		charIndex++;

		//spawnedObjList.Add (newCube);
	}

}

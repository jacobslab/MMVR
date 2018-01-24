using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlaceManager : MonoBehaviour {

	public GameObject cubePrefab;
	public GameObject charPrefab;
	[SerializeField]
	public ObjectPanelManager objPanelManager;
	public HierarchyManager hierarchyManager;

	private int cubeIndex=0;
	private int charIndex=0;

	// Use this for initialization
	void Start () {
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
		newObj.gameObj.GetComponent<Renderer> ().material.color = Color.red;
		//GameObject newCube = Instantiate (cubePrefab, Camera.main.transform.position + Camera.main.transform.forward * 10f, Quaternion.identity) as GameObject;
		//newCube.GetComponent<Renderer> ().material.color = Color.red;
		hierarchyManager.spawnedObjList.Add (newObj);
		GameObject newText = objPanelManager.AddTextObject (name,newObj);
		hierarchyManager.AddDictEntry (newText, newObj);
		cubeIndex++;
	}

	public void CreateCharacter()
	{
		string name = "character_" + charIndex.ToString ();
		SpawnableObject newObj = new SpawnableObject (name, Camera.main.transform.position + Camera.main.transform.forward * 10f,Vector3.zero,SpawnableObject.ObjectType.Character);
		newObj.gameObj.GetComponent<Renderer> ().material.color = Color.green;
		hierarchyManager.spawnedObjList.Add (newObj);
		GameObject newText = objPanelManager.AddTextObject (name,newObj);
		hierarchyManager.AddDictEntry (newText, newObj);
		charIndex++;

		//spawnedObjList.Add (newCube);
	}

}

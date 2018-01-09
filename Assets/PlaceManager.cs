using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceManager : MonoBehaviour {

	public GameObject cubePrefab;
	public GameObject charPrefab;
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
	}

	public void CreateCube()
	{
		GameObject newCube = Instantiate (cubePrefab, Camera.main.transform.position + Camera.main.transform.forward * 10f, Quaternion.identity) as GameObject;
		newCube.GetComponent<Renderer> ().material.color = Color.red;
	}

	public void CreateCharacter()
	{
		GameObject newCube = Instantiate (charPrefab, Camera.main.transform.position + Camera.main.transform.forward * 10f, Quaternion.identity) as GameObject;
		newCube.GetComponent<Renderer> ().material.color = Color.green;
	}

}

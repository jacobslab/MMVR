using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class SpawnableObject {

	public GameObject gameObj;
	public string objName;
	public Vector3 pos;
	public Vector3 rot;
	public string dictKey;
	public enum ObjectType
	{
		Terrain,
		PlayerStart,
		Building,
		Cube,
		Character,
		BoxObject,
		Text,

	}

	public ObjectType objType;

	public SpawnableObject(string name, Vector3 pos,Vector3 rot,ObjectType objType)
	{
		objName = name;
		this.pos = pos;
		this.rot = rot;
		this.objType = objType;
		switch (objType) {
		case ObjectType.Cube:
			gameObj = GameObject.Instantiate (Resources.Load<GameObject> ("Cube"));
			dictKey = "generic";
			break;
		case ObjectType.Character:
			gameObj = GameObject.Instantiate (Resources.Load<GameObject> ("Shelly"));
			dictKey = "generic";
			break;
		case ObjectType.Terrain:
			gameObj = GameObject.Instantiate (Resources.Load<GameObject> ("TerrainPrefab"));
			dictKey = "terrain";
			break;
		case ObjectType.PlayerStart:
			gameObj = GameObject.Instantiate (Resources.Load<GameObject> ("PlayerStartCube"));
			dictKey = "player_start";
			break;
		case ObjectType.Building:
			gameObj = GameObject.Instantiate (Resources.Load<GameObject> ("Building01"));
			dictKey = "building";
			break;
		}

		gameObj.transform.position = pos;
		gameObj.transform.eulerAngles = rot;
		gameObj.name = objName;
		if (objType != ObjectType.Terrain)
			gameObj.AddComponent<ObjectManipulator> ();
		objType = ObjectType.Cube;

	}
	public void UpdateValues ()
	{
		pos = gameObj.transform.position;
		rot = gameObj.transform.eulerAngles;
	}
	public void Destroy()
	{
		if(gameObj!=null)
			UnityEngine.Object.Destroy(gameObj);
	}

}

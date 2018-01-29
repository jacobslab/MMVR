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
	public enum ObjectType
	{
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
			gameObj = GameObject.CreatePrimitive (PrimitiveType.Cube);
			break;
		case ObjectType.Character:
			Debug.Log ("spawned character");
			gameObj = GameObject.Instantiate (Resources.Load<GameObject> ("Shelly"));
			break;
		}

		gameObj.transform.position = pos;
		gameObj.transform.eulerAngles = rot;
		gameObj.name = objName;
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

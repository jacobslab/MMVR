using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class SpawnableObject {

	public GameObject ok;
	public string objName;
	public Vector3 pos;
	public Vector3 rot;
	public enum ObjectType
	{
		Cube,
		Character
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
			ok = GameObject.CreatePrimitive (PrimitiveType.Cube);
			break;
		case ObjectType.Character:
			Debug.Log ("spawned character");
			ok = GameObject.Instantiate (Resources.Load<GameObject> ("Shelly"));
			break;
		}

		ok.transform.position = pos;
		ok.transform.eulerAngles = rot;
		ok.name = objName;
		ok.AddComponent<ObjectManipulator> ();
		objType = ObjectType.Cube;

	}
	public void UpdateValues ()
	{
		pos = ok.transform.position;
		rot = ok.transform.eulerAngles;
	}
	public void Destroy()
	{
		if(ok!=null)
			UnityEngine.Object.Destroy(ok);
	}

}

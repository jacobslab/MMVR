using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class SpawnableObject {

	public GameObject ok;
	public string objName;
	public Vector3 pos;
	public enum ObjectType
	{
		Cube,
		Character
	}

	public ObjectType objType;

	public SpawnableObject(string name, Vector3 pos,ObjectType objType)
	{
		objName = name;
		this.pos = pos;
		switch (objType) {
		case ObjectType.Cube:
			ok = GameObject.CreatePrimitive (PrimitiveType.Cube);
			break;
		case ObjectType.Character:
			ok = GameObject.Instantiate (Resources.Load<GameObject> ("Shelly"));
			break;
		}

		ok.transform.position = pos;
		ok.name = objName;
		ok.AddComponent<ObjectManipulator> ();
		objType = ObjectType.Cube;

	}
	public void UpdateValues ()
	{
		pos = ok.transform.position;
	}
	public void Destroy()
	{
		if(ok!=null)
			UnityEngine.Object.Destroy(ok);
	}

}

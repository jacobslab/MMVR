using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityFunctions : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public static Vector3 GetMousePosInWorldCoords()
	{
		Vector3 res= new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10f);
		return res;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA.Examples;
public class EditorCamController : MonoBehaviour {
	GameObject selectedObj;
	OrbitMouse mouseOrbit;


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
		mouseOrbit = GetComponent<OrbitMouse> ();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void SetSelectedObject(GameObject objSelected)
	{
		selectedObj = objSelected;
		if (selectedObj != null)
			mouseOrbit.target = selectedObj.transform;
	}

}

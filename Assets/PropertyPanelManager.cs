using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyPanelManager : MonoBehaviour {

	public GameObject terrainPropertiesPrefab;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddPropertyPanel(SpawnableObject.ObjectType objType,GameObject associatedObj)
	{
		switch (objType) {
		case SpawnableObject.ObjectType.Terrain:
			GameObject terrainPropertiesObj = Instantiate (terrainPropertiesPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			terrainPropertiesObj.transform.parent = transform.GetChild (0).transform;
			terrainPropertiesObj.transform.localPosition = Vector3.zero;
			terrainPropertiesObj.GetComponent<TerrainProperties> ().terrainObj = associatedObj;
			break;
		}
	}
}

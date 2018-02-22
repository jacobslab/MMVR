using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyPanelManager : MonoBehaviour {

	public GameObject skyboxPropertiesPrefab;
	public GameObject terrainPropertiesPrefab;
	public Dictionary<string,GameObject> propertyPanelDict;
	// Use this for initialization
	void Start () {
		propertyPanelDict= new Dictionary<string,GameObject> ();
		AddSkyboxPanel ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	void AddSkyboxPanel()
	{
		GameObject skyboxProperties = Instantiate (skyboxPropertiesPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		skyboxProperties.transform.parent = transform.GetChild (0).transform;
		skyboxProperties.transform.localPosition = Vector3.zero;
		propertyPanelDict.Add ("skybox", skyboxProperties);
		skyboxProperties.SetActive (false);
	}

	public void AddPropertyPanel(SpawnableObject.ObjectType objType,GameObject associatedObj)
	{
		switch (objType) {
		case SpawnableObject.ObjectType.Terrain:
			GameObject terrainPropertiesObj = Instantiate (terrainPropertiesPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			terrainPropertiesObj.transform.parent = transform.GetChild (0).transform;
			terrainPropertiesObj.transform.localPosition = Vector3.zero;
			terrainPropertiesObj.GetComponent<TerrainProperties> ().terrainObj = associatedObj;
			propertyPanelDict.Add ("terrain", terrainPropertiesObj);
			terrainPropertiesObj.SetActive (false);
			break;
		}
	}

	public void SwitchToPanel(string panelKey)
	{
		GameObject resultObj;
		bool result = propertyPanelDict.TryGetValue (panelKey, out resultObj);
		if (result) {
			resultObj.SetActive (true);
		}
	}
}

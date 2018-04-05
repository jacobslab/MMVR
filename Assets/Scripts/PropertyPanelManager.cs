using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Characters.ThirdPerson;
public class PropertyPanelManager : MonoBehaviour {

	public GameObject playerStartPropertiesPrefab;
	public GameObject genericPropertiesPrefab;
	public GameObject skyboxPropertiesPrefab;
	public GameObject terrainPropertiesPrefab;
	public Dictionary<string,GameObject> propertyPanelDict;
	public Vector3 spawnPos;
	private GameObject currentActivePanel;
	// Use this for initialization
	void Awake () {
		propertyPanelDict= new Dictionary<string,GameObject> ();
		AddSkyboxPanel ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	void AddSkyboxPanel()
	{
		GameObject skyboxProperties = Instantiate (skyboxPropertiesPrefab, spawnPos, Quaternion.identity) as GameObject;
//		skyboxProperties.transform.parent = transform.GetChild (0).transform;
		skyboxProperties.transform.parent = transform;
		skyboxProperties.transform.localPosition =spawnPos;
		skyboxProperties.transform.localScale = Vector3.one;
		propertyPanelDict.Add ("skybox", skyboxProperties);
		skyboxProperties.SetActive (false);
	}

	public void AddPropertyPanel(SpawnableObject.ObjectType objType,GameObject associatedObj)
	{
		switch (objType) {
		case SpawnableObject.ObjectType.Terrain:
			GameObject terrainPropertiesObj = CreatePropertyPanel (terrainPropertiesPrefab,associatedObj.name);
			terrainPropertiesObj.GetComponent<TerrainProperties> ().terrainObj = associatedObj;
			break;
		case SpawnableObject.ObjectType.PlayerStart:
			GameObject playerPropertiesObj = CreatePropertyPanel (playerStartPropertiesPrefab,associatedObj.name);
			break;
		default:
			GameObject cubePropertiesObj = CreatePropertyPanel (genericPropertiesPrefab, associatedObj.name);
			cubePropertiesObj.GetComponent<GenericProperties> ().associatedObj = associatedObj;
			break;
		}
	}

	GameObject CreatePropertyPanel(GameObject propertiesPrefab,string keyToAdd)
	{
		GameObject propertiesObj = Instantiate (propertiesPrefab, spawnPos, Quaternion.identity) as GameObject;
		propertiesObj.transform.parent = transform.transform;
		propertiesObj.transform.localPosition =spawnPos;
		propertiesObj.transform.localScale = Vector3.one;
		Debug.Log ("key to add: " + keyToAdd);
		Debug.Log ("propertiesobj: " + propertiesObj.name);
		propertyPanelDict.Add (keyToAdd, propertiesObj);
		propertiesObj.SetActive (false);
		return propertiesObj;
	}

	public void ApplyPlayerProperties(FirstPersonController fpsController)
	{
		GameObject resultObj;
		propertyPanelDict.TryGetValue ("player_start", out resultObj);
		fpsController.canControl = resultObj.GetComponent<PlayerStartProperties> ().playerControlToggle.isOn;
		fpsController.gameObject.GetComponent<CharacterController> ().enabled = resultObj.GetComponent<PlayerStartProperties> ().playerControlToggle.isOn;
		fpsController.EnableHandlebars (resultObj.GetComponent<PlayerStartProperties> ().hasHandlebarsToggle.isOn);
			
	}

	public void SwitchToPanel(string panelKey)
	{
//		Debug.Log ("attempting to switch panels");
		if (currentActivePanel != null)
			currentActivePanel.SetActive (false);
		GameObject resultObj;
		bool result = propertyPanelDict.TryGetValue (panelKey, out resultObj);
		if (result) {
			resultObj.SetActive (true);
			currentActivePanel = resultObj;
		}
	}
}

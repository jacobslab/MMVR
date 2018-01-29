using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicNodeManager : MonoBehaviour {

	public GameObject functionBoxPrefab;
	public GameObject utilityBoxPrefab;
	public Transform logicParent;
	public List<GameObject> functionBoxList;


	private int index;
	// Use this for initialization
	void Start () {
		functionBoxList = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateFunctionBox()
	{
		GameObject spawnedBox = Instantiate(functionBoxPrefab,Camera.main.ScreenToWorldPoint(UtilityFunctions.GetMousePosInWorldCoords()),Quaternion.identity) as GameObject;
		spawnedBox.transform.SetParent(logicParent,false);
		spawnedBox.GetComponent<FunctionBox> ().SetupFunctionBox ("Function_" + index.ToString ());
		functionBoxList.Add (spawnedBox);
		index++;
	}

	public void CreateUtilityBox(int utilityType)
	{
		GameObject spawnedBox = Instantiate(utilityBoxPrefab,Camera.main.ScreenToWorldPoint(UtilityFunctions.GetMousePosInWorldCoords()),Quaternion.identity) as GameObject;
		spawnedBox.transform.SetParent(logicParent,false);
		switch (utilityType) {
		case 0:
			spawnedBox.GetComponent<UtilityBox> ().SetupUtilityBox ("Print String", UtilityBox.UtilityType.PrintString);
			break;
		}
	}
}

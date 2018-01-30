using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicNodeManager : MonoBehaviour {

	public GameObject functionBoxPrefab;
	public GameObject utilityBoxPrefab;
	public Transform logicParent;
	public List<GameObject> functionBoxList;

	public List<GameObject> utilityList;


	private int index;
	// Use this for initialization
	void Start () {
		functionBoxList = new List<GameObject> ();
//		utilityList = new List<GameObject> ();
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
		switch (utilityType) {
		case 0:
			GameObject spawnedBox = Instantiate(utilityList[utilityType],Camera.main.ScreenToWorldPoint(UtilityFunctions.GetMousePosInWorldCoords()),Quaternion.identity) as GameObject;
			spawnedBox.transform.SetParent(logicParent,false);
			break;
		}
	}
}

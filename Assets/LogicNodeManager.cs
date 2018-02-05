using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class LogicNodeManager : MonoBehaviour {

	public GameObject functionBoxPrefab;
	public GameObject utilityBoxPrefab;
	public Transform logicParent;
	public List<GameObject> functionBoxList;

	public GameObject[] utilityList;

	public List<FunctionBox> funcSequence;

	private int index;
	// Use this for initialization
	void Start () {
		functionBoxList = new List<GameObject> ();
		utilityList = Resources.LoadAll<GameObject>("Logic/Utilities");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R))
			CreateSequenceOfFunctionBox ();
	}

	List<FunctionBox> CreateSequenceOfFunctionBox()
	{
		//sort function boxes from left to right
		List<GameObject> sortedList=functionBoxList.OrderBy (o => o.transform.position.x).ToList ();
		List<FunctionBox> sortedCompList = new List<FunctionBox> ();
		for (int i = 0; i < sortedList.Count; i++) {
			sortedCompList.Add(sortedList [i].GetComponent<FunctionBox>());
		}
		return sortedCompList;
	}

	public void ExecuteLogic()
	{
		funcSequence = CreateSequenceOfFunctionBox ();
		StartCoroutine (PerformLogicSequence (funcSequence));
	}

	IEnumerator PerformLogicSequence(List<FunctionBox> funcSequence)
	{
		//executes the sequence of each function box in the sorted order
		for (int i = 0; i < funcSequence.Count; i++) {
			yield return StartCoroutine (funcSequence [i].ExecuteSequence ());
		}
		
		yield return null;
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

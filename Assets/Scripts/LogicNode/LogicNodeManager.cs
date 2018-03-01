using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class LogicNodeManager : MonoBehaviour {

	public GameObject functionBoxPrefab;
	public GameObject utilityBoxPrefab;
	public Transform logicParent;
	public List<GameObject> functionBoxList;

	public GameObject[] utilityList;
	public Dropdown utilityDropdown;

	public Canvas logicCanvas;
	public GameObject variableContent;
	public List<GameObject> variableList;

	public GameObject varPlayground;
	int variableIndex=0;

	public List<FunctionBox> funcSequence;

	public GameObject variableGroupPrefab;

	private int index;
	float canvasScale=1f;
	// Use this for initialization
	void Start () {
		functionBoxList = new List<GameObject> ();
		utilityList = Resources.LoadAll<GameObject>("Logic/Utilities");
		variableList = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R))
			CreateSequenceOfFunctionBox ();

	}

//	void OnGUI()
//	{
//		Event currentEvent = Event.current;
//		if (currentEvent.type == EventType.ScrollWheel) {
//			if (currentEvent.delta.y > 0f) {
//				canvasScale += 0.01f;
//
//			} else
//				canvasScale -= 0.01f;
//			canvasScale=Mathf.Clamp (canvasScale, 0.2f, 3f);
//			logicCanvas.GetComponent<CanvasScaler>().scaleFactor = canvasScale;
//		}
//	}
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

	//executed by button "EXECUTE BUTTON"
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

	public void CreateNewVariable()
	{
		GameObject variableGroupObj = Instantiate (variableGroupPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		variableGroupObj.transform.parent = variableContent.transform;
		variableGroupObj.GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (91.62f, 240f + (variableIndex * -90f), 0f);
		variableGroupObj.GetComponent<VariablePanel> ().varPlaygroundRef = varPlayground;
		variableList.Add (variableGroupObj);
		variableIndex++;
	}


	public void CreateFunctionBox()
	{
		GameObject spawnedBox = Instantiate(functionBoxPrefab,Camera.main.ScreenToWorldPoint(UtilityFunctions.GetMousePosInWorldCoords()),Quaternion.identity) as GameObject;
		spawnedBox.transform.SetParent(logicParent,false);
		spawnedBox.GetComponent<FunctionBox> ().SetupFunctionBox ("Function_" + index.ToString ());
		functionBoxList.Add (spawnedBox);
		index++;
	}

	public void CreateUtilityBox()
	{
			GameObject spawnedBox = Instantiate(utilityList[utilityDropdown.value],Camera.main.ScreenToWorldPoint(UtilityFunctions.GetMousePosInWorldCoords()),Quaternion.identity) as GameObject;
			spawnedBox.transform.SetParent(logicParent,false);
	}
}

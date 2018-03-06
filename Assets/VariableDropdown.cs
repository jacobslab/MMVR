using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VariableDropdown : MonoBehaviour {

	public VariablePanel.VariableType varType;
	public List<GameObject> selectedVarList;
	List<string> varNamesList;
	Dropdown optionDropdown;
	// Use this for initialization
	void Start () {
		selectedVarList = new List<GameObject> ();
		optionDropdown = GetComponent<Dropdown> ();
		varNamesList = new List<string> ();
		BuildVariableList ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void BuildVariableList()
	{
		List<GameObject> variableList = LogicNodeManager.Instance.variableList;
		for(int i=0;i<variableList.Count;i++)
		{
			Debug.Log ("Comparing: " + variableList [i].GetComponent<VariablePanel> ().varType.ToString () + " and " + varType.ToString ());
			if (variableList [i].GetComponent<VariablePanel> ().varType == varType) {
				selectedVarList.Add (variableList [i]);
			}
		}
		GenerateStringList ();
		PopulateDropdown ();
	}
	void GenerateStringList()
	{
		for(int i=0;i<selectedVarList.Count;i++)
		{
			varNamesList.Add (selectedVarList [i].GetComponent<VariablePanel> ().variableName.text);
		}
	}

	void PopulateDropdown()
	{
		optionDropdown.ClearOptions ();
		if(varNamesList.Count>0)
			optionDropdown.AddOptions (varNamesList);
	}
}

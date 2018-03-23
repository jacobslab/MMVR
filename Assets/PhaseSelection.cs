using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PhaseSelection : MonoBehaviour {

	public Dropdown phaseLayerDropdown;
	List<string> phaseLayerList;
	int phaseIndex=0;
	// Use this for initialization
	void Start () {
		phaseLayerList = new List<string> ();
		CreateNewPhase ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateNewPhase()
	{
		string newPhaseName = "phase_" + phaseIndex.ToString ();
		LogicNodeManager.Instance.SpawnBasicLayer (newPhaseName);
		phaseIndex++;
		phaseLayerList.Add (newPhaseName);
		SwitchToPhaseLayer (newPhaseName);
		phaseLayerDropdown.ClearOptions ();
		phaseLayerDropdown.AddOptions (phaseLayerList);
	}

	public void SwitchToPhaseLayer(string targetPhase)
	{
		LogicNodeManager.Instance.SwitchToLogicLayer (targetPhase);
	}

	public void SwitchToPhaseLayer()
	{
		int chosenPhase = phaseLayerDropdown.value;
		//retrieve the name of the phase from the list
		string targetPhaseName =phaseLayerList[chosenPhase];
		SwitchToPhaseLayer (targetPhaseName);
	}
}

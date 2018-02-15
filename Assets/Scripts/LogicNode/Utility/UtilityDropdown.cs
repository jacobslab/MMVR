using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UtilityDropdown : MonoBehaviour {

	public GameObject[] utilityList;
	private List<string> utilityNameList;
	private Dropdown dropdown;
	// Use this for initialization
	void Start () {

		utilityList = Resources.LoadAll<GameObject>("Logic/Utilities");
		utilityNameList = new List<string> ();
		for (int i = 0; i < utilityList.Length; i++) {
			utilityNameList.Add (utilityList [i].GetComponent<UtilityBox>().displayName);
		}
		dropdown = GetComponent<Dropdown> ();
		PopulateDropdown ();

	}

	void PopulateDropdown()
	{
		//clear any options first
		dropdown.ClearOptions ();
		//then add based on all the available utilities
		dropdown.AddOptions(utilityNameList);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AssetPreviewer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject[] placeableObjArr = Resources.LoadAll<GameObject> ("Placeables");


	//	GetComponent<RawImage>().texture =AssetPreview.GetAssetPreview (placeableObjArr [0]);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

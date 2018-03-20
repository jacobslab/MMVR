using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
public class MorphAnimator : MonoBehaviour {

	public SkinnedMeshRenderer skinnedMeshRenderer;
	int prevMorphIndex=-1;
	public Dictionary<string,int> visemeDict;
	// Use this for initialization
	void Start () {
		visemeDict = new Dictionary<string,int> ();
		PopulateVisemeDict ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void PopulateVisemeDict()
	{
		for (int i = 0; i < skinnedMeshRenderer.sharedMesh.blendShapeCount; i++) {
			visemeDict.Add (skinnedMeshRenderer.sharedMesh.GetBlendShapeName (i), i);
		}
	}

	public void AnimateMorph(string morphName)
	{
		int morphIndex = -1;
		switch (morphName) {
		case "rest":
			morphName = "";
			break;
		case "E":
			morphName = "head__eCTRLvEH";
			break;
		case "FV":
			morphName = "head__eCTRLvF";
			break;
		case "MBP":
			morphName = "head__eCTRLvM";
			break;
		case "U":
			morphName = "head__eCTRLvUW";
			break;
		case "AI":
			morphName = "headhead__eCTRLvAA";
			break;
		case "etc":
			morphName = "head__eCTRLvT";
			break;
		case "L":
			morphName = "head__eCTRLvL";
			break;
		case "OW":
			morphName = "head__eCTRLvOW";
			break;
		case "WQ":
			morphName = "head__eCTRLvW";
			break;
		}

		Debug.Log ("animating morph " + morphName);
		bool result = visemeDict.TryGetValue (morphName, out morphIndex);
		if(result)
		{
			skinnedMeshRenderer.SetBlendShapeWeight (morphIndex, 100f);
			if (prevMorphIndex != -1) {
				skinnedMeshRenderer.SetBlendShapeWeight (prevMorphIndex, 0f);
			}
			prevMorphIndex = morphIndex;
			
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Linq; // used for Sum of array
using UnityEditor;
using System.Collections.Generic;
using System.IO;
public class ModifyTerrain : MonoBehaviour {



	private enum TerrainModes
	{
		Additive,
		Subtractive
	}
	TerrainModes terrainMode;

	private Terrain terrain;
	private Vector3 terrainPos;
	private TerrainData terrainData;
	float[, ,] splatmapData;
	float[,] heightmapData;
	int[,] detailmapData;
	public GameObject treePrefab;

	int treeLayer= 1<<10;
	void Start () {
		// Get the attached terrain component
		terrain = gameObject.GetComponent<Terrain> ();
		terrainPos = transform.position;
		// Get a reference to the terrain data
		terrainData = terrain.terrainData;
		splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
		heightmapData = new float[terrainData.heightmapWidth, terrainData.heightmapHeight];
		detailmapData = new int[terrainData.detailWidth, terrainData.detailHeight];

		ResetTerrain ();

	}


	void Update()
	{
		if (Input.GetMouseButton(0) && MMVR_Core.Instance.currentMode == MMVR_Core.Mode.SandboxEditor) {
			if (EditorCamController.Instance.GetSelectedObject () != null) {
				if (EditorCamController.Instance.GetSelectedObject ().name == "terrain") {
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out hit, 1000)) {
						Vector2 normPoints = new Vector2 (hit.point.x - terrainPos.x, hit.point.z - terrainPos.z);
						if (TerrainProperties.terrainMode == TerrainProperties.TerrainMode.Paint)
							ChangeSplatmap (normPoints);
						if (Input.GetKey (KeyCode.LeftShift)) {
							if (TerrainProperties.terrainMode == TerrainProperties.TerrainMode.Height)
								ChangeHeightmap (normPoints, TerrainModes.Subtractive);
							else if (TerrainProperties.terrainMode == TerrainProperties.TerrainMode.Detail)
								ChangeDetailMap (normPoints, TerrainModes.Subtractive);
							else if (TerrainProperties.terrainMode == TerrainProperties.TerrainMode.Tree)
								PaintTrees (hit.point, TerrainModes.Subtractive);
						} else {
							if (TerrainProperties.terrainMode == TerrainProperties.TerrainMode.Height)
								ChangeHeightmap (normPoints, TerrainModes.Additive);
							else if (TerrainProperties.terrainMode == TerrainProperties.TerrainMode.Detail)
								ChangeDetailMap (normPoints, TerrainModes.Additive);
							else if (TerrainProperties.terrainMode == TerrainProperties.TerrainMode.Tree)
								PaintTrees (hit.point, TerrainModes.Additive);
					
						}
					}
				}
			}
	}
	}

	void ResetTerrain()
	{
		ResetHeightmap ();
		ResetDetailMap ();
	}

	//trees
	void PaintTrees(Vector3 hitpoint,TerrainModes terrainMode)
	{
		if (terrainMode == TerrainModes.Additive) {
			GameObject treeInst = Instantiate (treePrefab, hitpoint, Quaternion.identity) as GameObject;
			treeInst.layer = 10;
		} else {
				RaycastHit hitTree;
			if(Physics.SphereCast(hitpoint,10f,Vector3.forward,out hitTree,10f,treeLayer))
			{
				Debug.Log ("found");
				if (hitTree.collider.gameObject != null) {
					Destroy (hitTree.collider.gameObject);
				}
			}
		}

	}


	//detail maps
	void ResetDetailMap()
	{
		detailmapData = terrainData.GetDetailLayer (0, 0, terrainData.detailWidth, terrainData.detailHeight, 0);
		Debug.Log ("size: " + detailmapData.Length.ToString ());
		for (int x = 0; x < terrainData.detailWidth; x++) {
			for (int y = 0; y < terrainData.detailHeight; y++) {
				detailmapData [x, y] = 0;
			}
		}
		terrainData.SetDetailLayer (0, 0, 0, detailmapData);
	}

	void ChangeDetailMap(Vector2 normPoints,TerrainModes terrainMode)
	{
		detailmapData = terrainData.GetDetailLayer (0, 0, terrainData.detailWidth, terrainData.detailHeight, 0);

		int mapX = Mathf.RoundToInt((normPoints.x/terrainData.size.x)*terrainData.detailWidth);
		int mapZ = Mathf.RoundToInt((normPoints.y / terrainData.size.z) * terrainData.detailHeight);
		for (int x = (mapX-5); x < (mapX+5); x++) {
			for (int y = (mapZ-5); y < (mapZ+5); y++) {
				if (terrainMode == TerrainModes.Additive)
					detailmapData [y, x] = 1;
				else
					detailmapData [y, x] = 0;
			}
		}
		terrainData.SetDetailLayer (0, 0, 0, detailmapData);
	}


	//height maps
	void ResetHeightmap()
	{
		heightmapData = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);
		for (int y = 0; y < terrainData.heightmapHeight; y++) {
			for (int x = 0; x < terrainData.heightmapWidth; x++) {
				heightmapData [y, x] = 0f;
			}
		}
		terrainData.SetHeightsDelayLOD (0, 0, heightmapData);
		terrain.ApplyDelayedHeightmapModification ();
	}

	void ChangeHeightmap(Vector2 normPoints,TerrainModes terrainMode)
	{

		heightmapData = terrainData.GetHeights(0, 0, terrainData.heightmapWidth, terrainData.heightmapHeight);

		int mapX = Mathf.RoundToInt((normPoints.x/terrainData.size.x)*terrainData.heightmapWidth);
		int mapZ = Mathf.RoundToInt((normPoints.y / terrainData.size.z) * terrainData.heightmapHeight);

		for (int y = mapZ-5; y < (mapZ+5); y++)
		{
			for (int x = mapX-5 ; x < (mapX+5); x++)
			{
				if (terrainMode == TerrainModes.Additive) {
					heightmapData [y, x] += 0.001f;
				} else {
					heightmapData [y, x] -= 0.001f;
				}
			}
		}
		terrainData.SetHeightsDelayLOD (0, 0, heightmapData);
	    terrain.ApplyDelayedHeightmapModification ();
	}

	//splat maps
	void ChangeSplatmap(Vector2 normPoints)
	{
		//normalize and multiply by alphamap to get the exact terrain coordinates that were clicked on
		int mapX = Mathf.RoundToInt((normPoints.x/terrainData.size.x)*terrainData.alphamapWidth);
		int mapZ = Mathf.RoundToInt((normPoints.y / terrainData.size.z) * terrainData.alphamapHeight);

		//then apply the appropriate splatmap
		for(int i=mapX-5;i<(mapX+5);i++)
		{
			for (int j = mapZ - 5; j < (mapZ + 5); j++) {
				for (int k = 0; i < 4; k++) {
					splatmapData [j, i, k] = 0f;
					terrainData.SetAlphamaps(0, 0, splatmapData);
				}
				splatmapData [j,i, TerrainProperties.targetSplatTextureIndex] = 0.3f;
			}
		}
		// Finally assign the new splatmap to the terrainData:
		terrainData.SetAlphamaps(0, 0, splatmapData);
	}


}
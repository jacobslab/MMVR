using UnityEngine;
using System.Collections;
using System.Linq; // used for Sum of array

public class AssignSplatMap : MonoBehaviour {

	private Terrain terrain;
	private Vector3 terrainPos;
	private TerrainData terrainData;
	float[, ,] splatmapData;
	void Start () {
		// Get the attached terrain component
		terrain = gameObject.GetComponent<Terrain> ();
		terrainPos = transform.position;
		// Get a reference to the terrain data
		terrainData = terrain.terrainData;
		splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

	}


	void Update()
	{
		if (Input.GetMouseButton(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 1000))
			{
				Debug.Log ("Hitpoint: " + hit.point.ToString ());
				Vector2 normPoints = new Vector2 (hit.point.x - terrainPos.x, hit.point.z- terrainPos.z);
				Debug.Log ("Normpoint: " + normPoints.ToString ());
				ChangeSplatmap (normPoints);
			}
		}
	}

	void ChangeSplatmap(Vector2 normPoints)
	{
//		// Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
//		float[, ,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
		float[,,] maps = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
		Debug.Log ("AlphamapHeight: " + terrainData.alphamapHeight.ToString());
		Debug.Log ("AlphamapWidth: " + terrainData.alphamapWidth.ToString ());
//		Debug.Log (maps.LongLength.ToString());
		int mapX = Mathf.RoundToInt((normPoints.x/terrainData.size.x)*terrainData.alphamapWidth);
		int mapZ = Mathf.RoundToInt((normPoints.y / terrainData.size.z) * terrainData.alphamapHeight);
		Debug.Log ("MapX " + mapX.ToString ());
		Debug.Log ("MapZ " + mapZ.ToString ());
//		splatmapData [mapX,mapZ, 3] = 1;
		for(int i=mapX-5;i<(mapX+5);i++)
		{
			for (int j = mapZ - 5; j < (mapZ + 5); j++) {
				splatmapData [j,i, 0] = 1;

			}
		}
//		for (int y = 0; y < terrainData.alphamapHeight; y++) {
//			for (int x = 0; x < terrainData.alphamapWidth; x++) {
//				splatmapData [x, y, 1] = 1;
//			}
//		}
//		for (int y = 0; y < terrainData.alphamapHeight/2; y++)
//		{
//			for (int x = 0; x < terrainData.alphamapWidth; x++)
//			{
//				// Normalise x/y coordinates to range 0-1 
//				float y_01 = (float)y/(float)terrainData.alphamapHeight;
//				float x_01 = (float)x/(float)terrainData.alphamapWidth;
//
//				// Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
//				float height = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapHeight),Mathf.RoundToInt(x_01 * terrainData.heightmapWidth) );
//
//				// Calculate the normal of the terrain (note this is in normalised coordinates relative to the overall terrain dimensions)
//				Vector3 normal = terrainData.GetInterpolatedNormal(y_01,x_01);
//
//				// Calculate the steepness of the terrain
//				float steepness = terrainData.GetSteepness(y_01,x_01);

				// Setup an array to record the mix of texture weights at this point
//				float[] splatWeights = new float[terrainData.alphamapLayers];
//				splatWeights [1] = 1;
//		splatmapData [Mathf.RoundToInt(normPoints.x),Mathf.RoundToInt(normPoints.y), 0] = 1;
				// CHANGE THE RULES BELOW TO SET THE WEIGHTS OF EACH TEXTURE ON WHATEVER RULES YOU WANT

				// Texture[0] has constant influence
				//				splatWeights[0] = 0.5f;
				//
				//				// Texture[1] is stronger at lower altitudes
				//				splatWeights[1] = Mathf.Clamp01((terrainData.heightmapHeight - height));
				//
				//				// Texture[2] stronger on flatter terrain
				//				// Note "steepness" is unbounded, so we "normalise" it by dividing by the extent of heightmap height and scale factor
				//				// Subtract result from 1.0 to give greater weighting to flat surfaces
				//				splatWeights[2] = 1.0f - Mathf.Clamp01(steepness*steepness/(terrainData.heightmapHeight/5.0f));
				//
				//				// Texture[3] increases with height but only on surfaces facing positive Z axis 
				//				splatWeights[3] = height * Mathf.Clamp01(normal.z);
				//
				//				// Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
				//				float z = splatWeights.Sum();
				//
				//				// Loop through each terrain texture
				//				for(int i = 0; i<terrainData.alphamapLayers; i++){
				//
				//					// Normalize so that sum of all texture weights = 1
				//					splatWeights[i] /= z;
				//
				//					// Assign this point to the splatmap array
				//					splatmapData[x, y, i] = splatWeights[i];
				//				}
//			}
//		}

		// Finally assign the new splatmap to the terrainData:
		terrainData.SetAlphamaps(0, 0, splatmapData);
	}
}
using UnityEngine;
using System.Collections;

public class TerrainDeformation_realtime : MonoBehaviour
{
	public Terrain myTerrain;
	public int xResolution;
	public int zResolution;
	float[,] heights;

	void Start()
	{
		xResolution = myTerrain.terrainData.heightmapWidth;
		zResolution = myTerrain.terrainData.heightmapHeight;
//		heights = myTerrain.terrainData.GetHeights(0,0,xResolution,zResolution);
		Debug.Log(myTerrain.terrainData.splatPrototypes.Length.ToString());
//				for (int i = 0; i < myTerrain.terrainData.splatPrototypes.Length; i++) {
//			Debug.Log (myTerrain.terrainData.splatPrototypes [i].texture.name);
//		}
	}

	void Update()
	{
//		if(Input.GetMouseButton(0))
//		{
//			RaycastHit hit;
//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//			if(Physics.Raycast(ray, out hit))
//			{
//				raiseTerrain(hit.point);
//			}
//		}
//		if(Input.GetMouseButton(1))
//		{
//			RaycastHit hit;
//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//			if(Physics.Raycast(ray, out hit))
//			{
//				lowerTerrain(hit.point);
//			}
//		}
	}

	private void raiseTerrain(Vector3 point)
	{
		Debug.Log ("raising terrain");
		int terX =(int)((point.x / myTerrain.terrainData.size.x) * xResolution);
		int terZ =(int)((point.z / myTerrain.terrainData.size.z) * zResolution);
		float y = heights[terX,terZ];
		y += 0.001f;
		float[,] height = new float[1,1];
		height[0,0] = y;
		heights[terX,terZ] = y;
		myTerrain.terrainData.SetHeights(terX, terZ, height);
	}

	private void lowerTerrain(Vector3 point)
	{
		Debug.Log ("lowering terrain");
		int terX =(int)((point.x / myTerrain.terrainData.size.x) * xResolution);
		int terZ =(int)((point.z / myTerrain.terrainData.size.z) * zResolution);
		float y = heights[terX,terZ];
		y -= 0.001f;
		float[,] height = new float[1,1];
		height[0,0] = y;
		heights[terX,terZ] = y;
		myTerrain.terrainData.SetHeights(terX, terZ, height);
	}
}
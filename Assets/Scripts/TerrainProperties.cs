using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainProperties : MonoBehaviour {
	public GameObject terrainObj;
	public Texture2D[] textureArr;
	public enum TerrainMode
	{
		Paint,
		Height,
		Detail
	}
	public static TerrainMode terrainMode;

	// Use this for initialization
	void Start () {
		textureArr = new Texture2D[transform.childCount];
		for (int i = 0; i < 4; i++) {
			textureArr [i] = (Texture2D)transform.GetChild (i).GetComponent<RawImage> ().texture;
		}
		
	}

	public void ActivatePaintMode()
	{
		terrainMode = TerrainMode.Paint;
	}

	public void ActivateHeightMode()
	{
		terrainMode = TerrainMode.Height;
	}
	public void ActivateDetailMode()
	{
		terrainMode = TerrainMode.Detail;
	}

	public void ChangeTerrainSplatTexture(int textureIndex)
	{
		Texture2D[] targetTexture = new Texture2D[1];
		targetTexture [0] = textureArr [textureIndex];
		Terrain terrain = terrainObj.GetComponent<Terrain> ();
		SetTerrainSplatMap (terrain, targetTexture);
	}

	void SetTerrainSplatMap(Terrain terrain, Texture2D[] textures)
	{
		var terrainData = terrain.terrainData;
		// The Splat map (Textures)
		SplatPrototype[] splatPrototype = new SplatPrototype[terrainData.splatPrototypes.Length];
		for (int i = 0; i < terrainData.splatPrototypes.Length; i++)
		{
			splatPrototype[i] = new SplatPrototype();
			splatPrototype[i].texture = textures[i];    //Sets the texture
			splatPrototype[i].tileSize = new Vector2(terrainData.splatPrototypes[i].tileSize.x, terrainData.splatPrototypes[i].tileSize.y);    //Sets the size of the texture
			splatPrototype[i].tileOffset = new Vector2(terrainData.splatPrototypes[i].tileOffset.x, terrainData.splatPrototypes[i].tileOffset.y);    //Sets the size of the texture
		}
		terrainData.splatPrototypes = splatPrototype;
	}
	// Update is called once per frame
	void Update () {
		
	}

	void OnApplicationQuit()
	{
		Destroy (terrainObj);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
public class PlacePanel : InteractableUIElement {
	public Transform canvasTransform;
	public RawImage previewImage; 
	public Transform originalParent;
	public Vector3 originalAnchoredPos3D;
	public SpawnableObject.ObjectType objectType;
	private Vector3 lastDraggedPos;
	public Text objName;
	// Use this for initialization
	void Start () {
		canvasTransform = GetComponentInParent<Canvas> ().transform;
		originalParent = transform.parent;
		originalAnchoredPos3D = GetComponent<RectTransform> ().anchoredPosition3D;
	}
	
	// Update is called once per frame
	void Update () {

	}

	//use this to setup the place panel
	public void SetupPanel(string name,GameObject gameObj)
	{
		objName.text = name;
//		previewImage.texture = AssetPreview.GetAssetPreview (gameObj);
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		transform.SetParent (canvasTransform, true);
	}
	void DropBackToPanel()
	{
		transform.SetParent(originalParent,true);
		GetComponent<RectTransform> ().anchoredPosition3D = originalAnchoredPos3D;
	}

	public override void OnBeginDrag(PointerEventData data)
	{
		selected = true;
		Debug.Log("They started dragging " + this.name);
	}

	public override void OnDrag(PointerEventData data)
	{
		GetComponent<RectTransform>().anchoredPosition3D = Camera.main.ScreenToViewportPoint (GetMousePosInWorldCoords());
		GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (GetComponent<RectTransform> ().anchoredPosition3D.x * Screen.width, GetComponent<RectTransform> ().anchoredPosition3D.y * Screen.height);
		lastDraggedPos = Camera.main.ScreenToWorldPoint (GetMousePosInWorldCoords());
	}

	public override void OnEndDrag(PointerEventData data)
	{
		DropSpawnObject ();
		Debug.Log ("deselected:" + gameObject.name);
		selected = false;
		DropBackToPanel ();
	}

	void DropSpawnObject()
	{
		switch (objectType) {
		case SpawnableObject.ObjectType.Cube:
			PlaceManager.Instance.CreateCube (lastDraggedPos);
			break;
		case SpawnableObject.ObjectType.Character:
			PlaceManager.Instance.CreateCharacter (lastDraggedPos);
			break;
		
		}
	}


}

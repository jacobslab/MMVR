using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PlacePanel : InteractableUIElement {
	public Transform canvasTransform;
	public Image previewImage; 
	public Transform originalParent;
	public Vector3 originalAnchoredPos3D;
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
	public void SetupPanel(string name, Sprite chosenImage)
	{
		objName.text = name;
		previewImage.sprite = chosenImage;
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
	public override void OnEndDrag(PointerEventData data)
	{

		Debug.Log ("deselected:" + gameObject.name);
		selected = false;
		DropBackToPanel ();
	}
	public override void OnDrag(PointerEventData data)
	{
		GetComponent<RectTransform>().anchoredPosition3D = Camera.main.ScreenToViewportPoint (new Vector3(Input.mousePosition.x,Input.mousePosition.y,10f));
		GetComponent<RectTransform> ().anchoredPosition3D = new Vector3 (GetComponent<RectTransform> ().anchoredPosition3D.x * Screen.width, GetComponent<RectTransform> ().anchoredPosition3D.y * Screen.height);

	}

}

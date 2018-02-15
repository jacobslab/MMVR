using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractableUIElement:  MonoBehaviour,IPointerEnterHandler,IPointerUpHandler, IPointerDownHandler,IPointerExitHandler,IBeginDragHandler, IDragHandler, IEndDragHandler {

	public bool selected=false;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (selected) {

			if (Input.GetKeyDown (KeyCode.Backspace)) {

				DestroyObject ();
			}
		}
	}
	public virtual void OnPointerEnter( PointerEventData eventData )
	{
	}
	public virtual void OnPointerDown( PointerEventData eventData )
	{

		selected=true;
		SelectObject();

	}
	public virtual void OnPointerUp( PointerEventData eventData )
	{
		selected=false;

	}

	public virtual void OnBeginDrag(PointerEventData data)
	{
		Debug.Log("They started dragging " + this.name);
	}
	public virtual void OnEndDrag(PointerEventData data)
	{
		Debug.Log("Stopped dragging " + this.name);
	}
	public virtual void OnDrag(PointerEventData data)
	{
		Debug.Log ("still dragging " + this.name); 
	}

	public void DestroyObject()
	{

	}
	public virtual void SelectObject()
	{
	}
	public virtual void DeselectObject()
	{
	}

	public virtual void OnPointerExit( PointerEventData eventData )
	{
		DeselectObject ();
	}

	public Vector3 GetMousePosInWorldCoords()
	{
		Vector3 res= new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10f);
		return res;
	}
}

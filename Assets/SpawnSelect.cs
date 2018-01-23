using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SpawnSelect : MonoBehaviour,IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler {

	public GameObject associatedObj;
	private bool selected=false;
	private Text textComp;
	// Use this for initialization
	void Start () {
		textComp = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (selected) {
			
			if (Input.GetKeyDown (KeyCode.Backspace)) {
				
				EditorCamController.Instance.Destroy ();
				Destroy (gameObject);
			}
		}
	}
	public void OnPointerEnter( PointerEventData eventData )
	{
	}
	public void OnPointerDown( PointerEventData eventData )
	{
//		Debug.Log ("click the text");
		textComp.color = Color.red;
		selected = true;
		EditorCamController.Instance.SetSelectedObject (associatedObj);
	}

	public void OnPointerExit( PointerEventData eventData )
	{
		textComp.color = Color.black;
	}
}

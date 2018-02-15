using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TransformAxes : MonoBehaviour {
	private GameObject parentObj;
	private Color originalColor;
	private Vector3 originalScale;
	private float originalDistance;
	private bool mouseDown=false;
	private Vector3 mouseDownPoint;
	public float factor=2f;
	private int axesID=-1; //0 is X, 1 is Y, 2 is Z

	public float rotFactor=4f;
	private bool changePos=false;
	private bool changeRot=false;
	private bool changeScale=false;

	private MeshFilter mesh;

	public Mesh transMesh;
	public Mesh rotMesh;
	public Mesh scaleMesh;

	public Text angleText;
	float mouseMove=0f;
	// Use this for initialization
	void Start () {
		originalColor= gameObject.GetComponent<Renderer> ().material.color;
		originalScale = transform.localScale;
		originalDistance = Vector3.Distance (Camera.main.transform.position, transform.position);
		parentObj = transform.parent.gameObject;
		switch (gameObject.name) {
		case "X":
			axesID = 0;
			break;
		case "Y":
			axesID = 1;
			break;
		case "Z":
			axesID = 2;
			break;
		default:
			axesID = 0;
			break;
		}
		changePos = true; //translate by default
		mesh = GetComponent<MeshFilter>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (mouseDown) {

			Vector3 worldPos;
			Vector3 temp = Input.mousePosition;
			temp.z = 10f;
			float mouseX = Input.GetAxis ("Mouse X");
			float mouseY = Input.GetAxis ("Mouse Y");
			worldPos = Camera.main.ScreenToWorldPoint (temp);
			mouseMove += mouseX;
//			Debug.Log ("mouseX: " + mouseMove.ToString ());
//			Debug.Log("angle: " + Vector3.Angle (worldPos, mouseDownPoint).ToString());
//			angleText.text = Vector3.Angle (worldPos, mouseDownPoint).ToString ();
//			UpdateRotation (worldPos);
			if (changePos)
				UpdatePosition (mouseX,mouseY);
			else if (changeRot)
				UpdateRotation(mouseX,mouseY);
			else if (changeScale)
				UpdateScale(mouseX,mouseY);
		} else
			mouseMove = 0f;
	}

	public void ChangeMode(int mode)
	{
		switch (mode) {
		case 0:
			angleText.text = "Translation";
			mesh.mesh = transMesh;
			changePos = true;
			changeRot = false;
			changeScale = false;
			break;
		case 1:
			angleText.text = "Rotation";
			mesh.mesh = rotMesh;
			changePos = false;
			changeRot = true;
			changeScale = false;
			break;
		case 2:
			angleText.text = "Scale";
			mesh.mesh = scaleMesh;
			changePos = false;
			changeRot = false;
			changeScale = true;
			break;
		default:
			angleText.text = "Translation";
			changePos = true;
			changeRot = false;
			changeScale = false;
			break;
		}
	}

	void UpdatePosition(float mouseX, float mouseY)
	{
		switch(axesID)
		{
		case 0:
//			Debug.Log ("updating posX");
			parentObj.transform.position = new Vector3 (parentObj.transform.position.x + (mouseX * factor), parentObj.transform.position.y, parentObj.transform.position.z);
			break;
		case 1:
//			Debug.Log ("updating posY");
			parentObj.transform.position = new Vector3 (parentObj.transform.position.x, parentObj.transform.position.y + (mouseY * factor), parentObj.transform.position.z);
			break;
		case 2:
			parentObj.transform.position = new Vector3 (parentObj.transform.position.x, parentObj.transform.position.y, parentObj.transform.position.z + ((mouseX + mouseY) * -factor));
			break;
//			float difference = worldPos.x - mouseDownPoint.x;
//			float angleFactor=Vector3.Angle (worldPos, mouseDownPoint);
//			parentObj.transform.position = new Vector3 (parentObj.transform.position.x, parentObj.transform.position.y, parentObj.transform.position.z + (angleFactor);
			break;

		}
	}

	void UpdateRotation(float mouseX, float mouseY)
	{
		switch(axesID)
		{
		case 0:
//			Debug.Log ("updating rotX");
			parentObj.transform.eulerAngles = new Vector3 (parentObj.transform.eulerAngles.x + (mouseY * rotFactor), parentObj.transform.eulerAngles.y, parentObj.transform.eulerAngles.z);
			break;
		case 1:
//			Debug.Log ("updating rotY");
			parentObj.transform.eulerAngles = new Vector3 (parentObj.transform.eulerAngles.x, parentObj.transform.eulerAngles.y + (mouseX * rotFactor), parentObj.transform.eulerAngles.z);
			break;
//		case 2:
//			float difference = worldPos.x - mouseDownPoint.x;
//			parentObj.transform.eulerAngles = new Vector3 (parentObj.transform.eulerAngles.x, parentObj.transform.eulerAngles.y, parentObj.transform.eulerAngles.z + (-0.025f*difference * rotFactor));
//			break;

		}
	}

	void UpdateScale(float mouseX, float mouseY)
	{
		switch(axesID)
		{
		case 0:

//			Debug.Log ("updating scaleX");
			parentObj.transform.localScale = new Vector3 (parentObj.transform.localScale.x + (mouseX * factor), parentObj.transform.localScale.y, parentObj.transform.localScale.z);
			break;
		case 1:
//			Debug.Log ("updating scaleY");
			parentObj.transform.localScale = new Vector3 (parentObj.transform.localScale.x, parentObj.transform.localScale.y + (mouseY * factor), parentObj.transform.localScale.z);
			break;
//		case 2:
//			float difference = worldPos.x - mouseDownPoint.x;
//			parentObj.transform.localScale = new Vector3 (parentObj.transform.localScale.x, parentObj.transform.localScale.y, parentObj.transform.localScale.z + (-0.025f*difference));
//			break;

		}
	}

	void UpdateAxesScale()
	{
		float currentDistance = Vector3.Distance (Camera.main.transform.position, transform.position);
		float difference = currentDistance - originalDistance;
//		Debug.Log ("difference is: " + difference.ToString ());
		transform.localScale= Vector3.Lerp(originalScale*factor,originalScale*factor,1f-(difference/Camera.main.farClipPlane));


	}

	void OnMouseDown()
	{
//		Debug.Log ("mouse down");
		mouseDownPoint=Camera.main.ScreenToWorldPoint (new Vector3(Input.mousePosition.x,Input.mousePosition.y,10f));
		gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
		mouseDown = true;
	}

	void OnMouseUp()
	{
//		Debug.Log ("mouse up");
		gameObject.GetComponent<Renderer> ().material.color = originalColor;
		mouseDown = false;
	}
}

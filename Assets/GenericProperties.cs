using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GenericProperties : MonoBehaviour {

	public InputField xPos;
	public InputField yPos;
	public InputField zPos;
	public InputField xRot;
	public InputField yRot;
	public InputField zRot;
	public InputField xScale;
	public InputField yScale;
	public InputField zScale;

	public Slider rColor;
	public Slider gColor;
	public Slider bColor;
	public Slider aColor;
	public Toggle enablePhysics;
	public Toggle enableCollision;

	public GameObject associatedObj;

	private Rigidbody rigidbody;
	// Use this for initialization
	void Start () {
		
	}



	void OnEnable()
	{
		if (associatedObj != null) {
			UpdateFields ();

			CheckPhysics ();
			CheckCollision ();
		} else 
		{
				rigidbody = associatedObj.GetComponent<Rigidbody> ();
		}
	}

	public void UpdateColors()
	{
		associatedObj.GetComponent<Renderer> ().material.color = new Color (rColor.value, gColor.value, bColor.value, aColor.value);
	}

	public void UpdateTransform()
	{
		Debug.Log ("updating transform");
		associatedObj.transform.position = new Vector3 (float.Parse (xPos.text), float.Parse (yPos.text), float.Parse (zPos.text));
		associatedObj.transform.eulerAngles = new Vector3 (float.Parse (xRot.text), float.Parse (yRot.text), float.Parse (zRot.text));
		associatedObj.transform.localScale = new Vector3 (float.Parse (xScale.text), float.Parse (yScale.text), float.Parse (zScale.text));
	}

	public void UpdateFields()
	{
		Debug.Log ("updating fields");
		xPos.text = associatedObj.transform.position.x.ToString ();
		yPos.text = associatedObj.transform.position.y.ToString ();
		zPos.text = associatedObj.transform.position.z.ToString ();

		xRot.text = associatedObj.transform.eulerAngles.x.ToString ();
		yRot.text = associatedObj.transform.eulerAngles.y.ToString ();
		zRot.text = associatedObj.transform.eulerAngles.z.ToString ();

		xScale.text = associatedObj.transform.localScale.x.ToString ();
		yScale.text = associatedObj.transform.localScale.y.ToString ();
		zScale.text = associatedObj.transform.localScale.z.ToString ();

		rColor.value = associatedObj.GetComponent<Renderer> ().material.color.r;
		gColor.value = associatedObj.GetComponent<Renderer> ().material.color.g;
		bColor.value = associatedObj.GetComponent<Renderer> ().material.color.b;
		aColor.value = associatedObj.GetComponent<Renderer> ().material.color.a;
	}


	public static bool HasComponent <T>(GameObject obj) where T:Component
	{
		return obj.GetComponent<T>() != null;
	}


	public void CheckPhysics()
	{
		if (enablePhysics.isOn) {
				associatedObj.GetComponent<Rigidbody> ().isKinematic = false;
			associatedObj.GetComponent<Rigidbody> ().useGravity = true;
		} else {
			associatedObj.GetComponent<Rigidbody> ().isKinematic =true;
			associatedObj.GetComponent<Rigidbody> ().useGravity = false;
		}
	}

	public void CheckCollision()
	{
		if (enableCollision.isOn)
			associatedObj.GetComponent<BoxCollider> ().enabled = true;
		else
			associatedObj.GetComponent<BoxCollider> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

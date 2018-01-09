using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicNode : MonoBehaviour {

	GameObject target;
	// Use this for initialization
	void Start () {
		target = this.gameObject;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public IEnumerator ChangeColor()
	{
		target.GetComponent<Renderer> ().material.color = Color.red;
		yield return null;
	}

	public IEnumerator MoveObject()
	{
		float factor = 0f;
		while (factor < 3f) {
			factor += Time.deltaTime;
			target.transform.position = Vector3.Lerp (target.transform.position, new Vector3 (7f, 4f, 0f), factor / 3f);
			yield return 0;
		}
		yield return null;
	}
}

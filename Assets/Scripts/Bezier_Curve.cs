using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Bezier_Curve : MonoBehaviour
{
	public Vector3 p0,p1,p2;
	public Color color = Color.white;
	public float width = 0.2f;
	public int numberOfPoints = 20;
	LineRenderer lineRenderer;
	public bool useWorldSpace=true;
	public RectTransform outPin;
	Vector3 lastClickedPos;
	public bool isSet=false;

	public GameObject start,end;
	void Start () 
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.useWorldSpace = useWorldSpace;
//		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
	}

	void FixedUpdate () 
	{
		if (!isSet) {
			if (Input.GetMouseButtonDown (0)) {
				lastClickedPos = GetMousePosInWorldCoords ();
			}
			if (lineRenderer == null) {
				return; // no points specified
			}

			// update line renderer
			lineRenderer.startColor = color;
			lineRenderer.endColor = color;
			lineRenderer.startWidth = width;
			lineRenderer.endWidth = width;

			if (numberOfPoints > 0) {
				lineRenderer.positionCount = numberOfPoints;
			}

			// set points of quadratic Bezier curve
//		Vector3 p0 =start.transform.position;
//		p0 =  Camera.main.ScreenToWorldPoint(lastClickedPos);
//		Vector3 p1 = middle.transform.position;

//		Vector3 p2 = end.transform.position;
			p2 = Camera.main.ScreenToWorldPoint (GetMousePosInWorldCoords ());
			UpdateCurve (p0, p2);
		} else {
//			Debug.Log ("post-update");
			UpdateCurve (Camera.main.ScreenToWorldPoint (start.transform.position), Camera.main.ScreenToWorldPoint (end.transform.position));
		}
	}

	public void UpdateCurve(Vector3 p0,Vector3 p2)
	{
		//if their values are zero, then they were set by "one-end" of the curve which means we should retain their old positions
		if (p0 == Vector3.zero)
			p0 = p0;
		else if (p2 == Vector3.zero)
			p2 = p2;
		p1 = (p0 + p2) * 0.5f;
		float t;
		Vector3 position;
		for(int i = 0; i < numberOfPoints; i++)
		{
			t = i / (numberOfPoints - 1.0f);
			position = (1.0f - t) * (1.0f - t) * p0 
				+ 2.0f * (1.0f - t) * t * p1 + t * t * p2;
			lineRenderer.SetPosition(i, position);
		}
	}



	public Vector3 GetMousePosInWorldCoords()
	{
		Vector3 res= new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10f);
		return res;
	}
}
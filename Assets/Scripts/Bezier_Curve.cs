using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Bezier_Curve : MonoBehaviour
{
	public GameObject start, middle, end;
	public Color color = Color.white;
	public float width = 0.2f;
	public int numberOfPoints = 20;
	LineRenderer lineRenderer;
	public bool useWorldSpace=true;
	public RectTransform outPin;
	Vector3 lastClickedPos;
	void Start () 
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.useWorldSpace = useWorldSpace;
//		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
	}

	void Update () 
	{
		if (Input.GetMouseButtonDown (0)) {
			lastClickedPos = GetMousePosInWorldCoords();
		}
		if( lineRenderer == null)
		{
			return; // no points specified
		}

		// update line renderer
		lineRenderer.startColor = color;
		lineRenderer.endColor = color;
		lineRenderer.startWidth = width;
		lineRenderer.endWidth = width;

		if (numberOfPoints > 0)
		{
			lineRenderer.positionCount = numberOfPoints;
		}

		// set points of quadratic Bezier curve
//		Vector3 p0 =start.transform.position;
		Vector3 p0 =  Camera.main.ScreenToWorldPoint(lastClickedPos);
//		Vector3 p1 = middle.transform.position;

//		Vector3 p2 = end.transform.position;
		Vector3 p2 = Camera.main.ScreenToWorldPoint(GetMousePosInWorldCoords());
		Vector3 p1 = (p0 + p2) * 0.25f;
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
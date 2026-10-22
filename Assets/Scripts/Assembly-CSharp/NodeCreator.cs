using UnityEngine;

[AddComponentMenu("")]
public class NodeCreator : MonoBehaviour
{
	public Spline spline;

	public SplineMesh splineMesh;

	private void Update()
	{
		Move();
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GameObject gameObject = spline.AddSplineNode();
			gameObject.transform.position = transform.position;
			splineMesh.segmentCount += 3;
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			SplineNode[] splineNodes = spline.SplineNodes;
			if (splineNodes.Length >= 1)
			{
				SplineNode splineNode = splineNodes[0];
				spline.RemoveSplineNode(splineNode);
				splineMesh.segmentCount -= 3;
			}
		}
	}

	private void Move()
	{
		if (Input.GetKey(KeyCode.W))
		{
			transform.position += Vector3.forward * Time.deltaTime * 4f;
		}
		if (Input.GetKey(KeyCode.S))
		{
			transform.position += Vector3.back * Time.deltaTime * 4f;
		}
		if (Input.GetKey(KeyCode.A))
		{
			transform.position += Vector3.left * Time.deltaTime * 4f;
		}
		if (Input.GetKey(KeyCode.D))
		{
			transform.position += Vector3.right * Time.deltaTime * 4f;
		}
	}
}

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("")]
public class BranchingSplineDemo : MonoBehaviour
{
	public BranchingSpline bSpline;

	public BranchingSplineParameter bParam = new BranchingSplineParameter();

	private void Update()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			bSpline.Advance(bParam, Time.deltaTime * 10f, JunctionController);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			bSpline.Advance(bParam, (0f - Time.deltaTime) * 10f, JunctionController);
		}
		transform.position = bSpline.GetPosition(bParam);
	}

	private BranchingSplinePath JunctionController(BranchingSplineParameter currentParameter, List<BranchingSplinePath> possiblePaths)
	{
		int index = (int)(Random.value * (float)possiblePaths.Count);
		return possiblePaths[index];
	}
}

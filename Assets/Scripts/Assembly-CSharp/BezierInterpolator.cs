public class BezierInterpolator : SplineInterpolator
{
	public BezierInterpolator()
	{
		CoefficientMatrix = new double[16]
		{
			-1.0, 3.0, -3.0, 1.0, 3.0, -6.0, 3.0, 0.0, -3.0, 3.0,
			0.0, 0.0, 1.0, 0.0, 0.0, 0.0
		};
		NodeIndices = new int[4] { 0, 1, 2, 3 };
	}
}

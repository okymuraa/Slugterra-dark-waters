public class LinearInterpolator : SplineInterpolator
{
	public LinearInterpolator()
	{
		CoefficientMatrix = new double[16]
		{
			0.0, 0.0, -1.0, 1.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0,
			0.0, 0.0, 0.0, 0.0, 0.0, 0.0
		};
		NodeIndices = new int[4] { 0, 1, 2, 3 };
	}
}

using UnityEngine;

public class GradientContainer : MonoBehaviour
{
	public Gradient ColorGradient;

	private static Color s_HighDensityColor = new Color(1f, 0f, 0f, 1f);

	private static Color s_MediumDensityColor = new Color(1f, 1f, 0f, 1f);

	private static Color s_LowDensityColor = new Color(0f, 1f, 1f, 1f);

	public GradientContainer()
	{
		ColorGradient = new Gradient();
		GradientColorKey[] array = new GradientColorKey[3];
		array[0].color = s_LowDensityColor;
		array[0].time = 0f;
		array[1].color = s_MediumDensityColor;
		array[1].time = 0.5f;
		array[2].color = s_HighDensityColor;
		array[2].time = 1f;
		GradientAlphaKey[] array2 = new GradientAlphaKey[3];
		array2[0].alpha = 0.2f;
		array2[0].time = 0f;
		array2[1].alpha = 0.5f;
		array2[1].time = 0.5f;
		array2[2].alpha = 1f;
		array2[2].time = 1f;
		ColorGradient.SetKeys(array, array2);
	}
}

using UnityEngine;

[AddComponentMenu("")]
public class AlterSineMeshModifier : MonoBehaviour
{
	public SplineSineScaleModifier meshModifier;

	private void Update()
	{
		meshModifier.offset += 2f * Time.deltaTime;
		meshModifier.sinMultiplicator = Mathf.Sin(Time.time) * 0.75f;
		meshModifier.sinMultiplicator = meshModifier.sinMultiplicator * meshModifier.sinMultiplicator + 0.5f;
	}
}

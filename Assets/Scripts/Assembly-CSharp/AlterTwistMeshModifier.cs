using UnityEngine;

[AddComponentMenu("")]
public class AlterTwistMeshModifier : MonoBehaviour
{
	public SplineTwistModifier meshModifier;

	private void Update()
	{
		meshModifier.twistOffset += 0.1f * Time.deltaTime;
	}
}

using System.Collections;
using UnityEngine;

[AddComponentMenu("Slugterra/Game/DestroyImmediateTimed")]
public class DestroyImmediateTimed : MonoBehaviour
{
	[SerializeField]
	public float v_Time = 5f;

	private void Start()
	{
		Object.Destroy(gameObject, v_Time);
	}

	private IEnumerator DestroyTimed()
	{
		yield return new WaitForSeconds(v_Time);
		Object.DestroyImmediate(gameObject);
		StopCoroutine("DestroyTimed");
	}
}

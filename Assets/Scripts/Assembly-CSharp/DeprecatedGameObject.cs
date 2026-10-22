using UnityEngine;

public class DeprecatedGameObject : MonoBehaviour
{
	private void Awake()
	{
		Debug.LogWarningFormat("<b><color=red>Object {0} is deprecated</color></b>", name);
	}
}

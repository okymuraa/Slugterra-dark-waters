using System.Collections;
using UnityEngine;

public class tk2dUIItemAutoClick : MonoBehaviour
{
	[SerializeField]
	private tk2dUIItem item;

	private IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		if ((bool)item)
		{
			item.SimulateClick();
		}
	}
}

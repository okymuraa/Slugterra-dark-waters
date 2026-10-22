using System;
using System.Collections;
using UnityEngine;

public class DelayUtility
{
	public static IEnumerator Waiting(float waitingSeconds, Action afterCallBack)
	{
		float expired = 0f;
		while (expired - waitingSeconds < 1E-05f)
		{
			expired += Time.deltaTime;
			yield return null;
		}
		if (afterCallBack != null)
		{
			afterCallBack();
		}
	}
}

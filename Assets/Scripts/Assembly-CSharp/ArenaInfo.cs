using UnityEngine;

public class ArenaInfo : MonoBehaviour
{
	private bool weitFrame;

	private Camera infoCam;

	private void Start()
	{
		infoCam = gameObject.GetComponent<Camera>();
	}

	private void LateUpdate()
	{
		if (Time.timeScale == 0f)
		{
			if (infoCam.enabled)
			{
				infoCam.enabled = false;
				weitFrame = true;
			}
		}
		else if (!infoCam.enabled)
		{
			if (weitFrame)
			{
				weitFrame = false;
			}
			else
			{
				infoCam.enabled = true;
			}
		}
	}
}

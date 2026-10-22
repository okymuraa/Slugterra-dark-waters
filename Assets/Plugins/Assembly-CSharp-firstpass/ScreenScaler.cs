using UnityEngine;

public class ScreenScaler : MonoBehaviour
{
	public bool calulateStartOnly = true;

	public float persentsY = 100f;

	private float _scaleFactorY = 1f;

	private float _xScaleDiff;

	private void Awake()
	{
		_scaleFactorY = transform.localScale.y;
		_xScaleDiff = transform.localScale.x / transform.localScale.y;
		if (calulateStartOnly)
		{
			placementCalculation();
		}
	}

	private void Update()
	{
		if (!calulateStartOnly)
		{
			placementCalculation();
		}
	}

	public void placementCalculation()
	{
		float num = (float)Screen.height / 100f * persentsY;
		Rect objectBounds = PreviewScreenUtil.getObjectBounds(gameObject);
		if (objectBounds.height < num)
		{
			while (objectBounds.height < num)
			{
				objectBounds = PreviewScreenUtil.getObjectBounds(gameObject);
				transform.localScale = new Vector3(_scaleFactorY * _xScaleDiff, _scaleFactorY, 0f);
				_scaleFactorY += 0.1f;
			}
		}
		else
		{
			while (objectBounds.height > num)
			{
				objectBounds = PreviewScreenUtil.getObjectBounds(gameObject);
				transform.localScale = new Vector3(transform.localScale.x - transform.localScale.x * 0.1f, transform.localScale.y - transform.localScale.y * 0.1f, transform.localScale.z);
			}
		}
	}
}

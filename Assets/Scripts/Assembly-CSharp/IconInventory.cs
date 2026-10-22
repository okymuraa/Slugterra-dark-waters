using UnityEngine;
using UnityEngine.EventSystems;

public class IconInventory : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IEventSystemHandler
{
	private static Transform EliModels;

	public Transform _crosshair;

	public Transform _crosshairUp;

	public Transform _crosshairCenter;

	public Transform pivotCamera;

	private Transform Canvas;

	private Vector3 EliRotation;

	private Vector3 CameraRotation;

	private Quaternion camRotationX;

	public float LerpSpeed = 2f;

	private bool _lock;

	private float offset;

	public void OnBeginDrag(PointerEventData eventData)
	{
		_lock = true;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		_lock = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (EliModels != null)
		{
			EliRotation = EliModels.localEulerAngles;
			EliRotation.y += eventData.delta.x * 0.35f;
			EliModels.localRotation = Quaternion.Euler(EliRotation);
		}
		if (pivotCamera != null)
		{
			CameraRotation = pivotCamera.localEulerAngles;
			CameraRotation.x -= eventData.delta.y * 0.35f;
			camRotationX = Quaternion.Euler(CameraRotation);
			if (camRotationX.x > 0.1f && camRotationX.x > 0.13f)
			{
				camRotationX.x = 0.13f;
			}
			if (camRotationX.x < -0.1f && camRotationX.x < -0.13f)
			{
				camRotationX.x = -0.13f;
			}
			pivotCamera.localRotation = camRotationX;
		}
	}

	private void LateUpdate()
	{
		offset = camRotationX.x;
		if (camRotationX.w < 0f)
		{
			offset = 0f;
		}
		_crosshair.position = Vector3.Lerp(_crosshairCenter.position, _crosshairUp.position, offset * 4f);
		if (!_lock)
		{
			if (camRotationX.x > 0.1f)
			{
				camRotationX.x = Mathf.Lerp(camRotationX.x, 0.1f, Time.deltaTime * LerpSpeed);
				pivotCamera.localRotation = camRotationX;
			}
			if (camRotationX.x < -0.1f)
			{
				camRotationX.x = Mathf.Lerp(camRotationX.x, -0.1f, Time.deltaTime * LerpSpeed);
				pivotCamera.localRotation = camRotationX;
			}
		}
	}

	public static void AddPlayer(Transform _eli)
	{
		EliModels = _eli;
	}
}

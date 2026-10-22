using System.Collections;
using Slugterra.UI.Game.Camera;
using UnityEngine;

public class Camera_AnimateByPoints : MonoBehaviour
{
	public GameObject _CameraToAnimate;

	public float _CameraReachPointDistanceDetect = 0.1f;

	public bool _isPlayerCamera;

	public GameObject[] _CameraPositions;

	public float[] _LerpSpeedToPos;

	public float[] _ConstSpeedToPos;

	public float[] _PauseTimeInPoint;

	public bool[] _Lerp_Constant_move;

	public string DEBUG_____G = "==================";

	private Camera _CameraComponent;

	private Camera[] _CameraComponentPoints;

	private Transform _CameraToAnimateTRS;

	private Transform[] _CameraPositionsTRS;

	private bool b_StartSequence = true;

	public int _TargetIndex;

	private Quaternion _StartRotation;

	private bool b_Paused;

	public CameraSmoothFollow _CameraController;

	private void Start()
	{
		b_Paused = false;
		if (_CameraToAnimate != null)
		{
			_CameraToAnimateTRS = _CameraToAnimate.transform;
			_CameraComponent = _CameraToAnimate.GetComponent<Camera>();
		}
		else
		{
			b_StartSequence = false;
		}
		if (_CameraPositions.Length > 0)
		{
			_CameraPositionsTRS = new Transform[_CameraPositions.Length];
			_CameraComponentPoints = new Camera[_CameraPositions.Length];
			for (int i = 0; i < _CameraComponentPoints.Length; i++)
			{
				_CameraComponentPoints[i] = _CameraPositions[i].GetComponent<Camera>();
			}
			for (int j = 0; j < _CameraPositionsTRS.Length; j++)
			{
				if (_CameraPositions[j] != null)
				{
					_CameraPositionsTRS[j] = _CameraPositions[j].transform;
					continue;
				}
				Debug.Log("!!!---Empty element in:Camera_AnimateByPoints");
				b_StartSequence = false;
			}
			if (_Lerp_Constant_move.Length < 1 && _CameraPositionsTRS.Length > 0)
			{
				_Lerp_Constant_move = new bool[_CameraPositions.Length];
			}
			if (_PauseTimeInPoint.Length < 1 && _CameraPositionsTRS.Length > 0)
			{
				_PauseTimeInPoint = new float[_CameraPositions.Length];
			}
		}
		else
		{
			b_StartSequence = false;
		}
		_StartRotation = _CameraToAnimateTRS.rotation;
	}

	private void Update()
	{
		if (!b_StartSequence)
		{
			return;
		}
		if (_TargetIndex < _CameraPositions.Length && !b_Paused && _CameraPositions[_TargetIndex] != null)
		{
			if ((_CameraPositionsTRS[_TargetIndex].position - _CameraToAnimateTRS.position).sqrMagnitude > _CameraReachPointDistanceDetect)
			{
				StartTransition();
			}
			else
			{
				NextPositionSwitch();
			}
		}
		else
		{
			NextPositionSwitch();
		}
	}

	private void StartTransition()
	{
		if (_Lerp_Constant_move[_TargetIndex])
		{
			if (_ConstSpeedToPos[_TargetIndex] > 1000f)
			{
				SwitchToPosition();
				return;
			}
			_CameraToAnimateTRS.position = Vector3.MoveTowards(_CameraToAnimateTRS.position, _CameraPositionsTRS[_TargetIndex].position, Time.deltaTime * _ConstSpeedToPos[_TargetIndex]);
			_CameraToAnimateTRS.rotation = Quaternion.RotateTowards(_CameraToAnimateTRS.rotation, _CameraPositionsTRS[_TargetIndex].rotation, Time.deltaTime * _ConstSpeedToPos[_TargetIndex]);
			_CameraComponent.fieldOfView = Mathf.MoveTowards(_CameraComponent.fieldOfView, _CameraComponentPoints[_TargetIndex].fieldOfView, Time.deltaTime * _ConstSpeedToPos[_TargetIndex]);
		}
		else if (_LerpSpeedToPos[_TargetIndex] > 1000f)
		{
			SwitchToPosition();
		}
		else
		{
			_CameraToAnimateTRS.position = Vector3.Lerp(_CameraToAnimateTRS.position, _CameraPositionsTRS[_TargetIndex].position, Time.deltaTime * _LerpSpeedToPos[_TargetIndex]);
			_CameraToAnimateTRS.rotation = Quaternion.Lerp(_CameraToAnimateTRS.rotation, _CameraPositionsTRS[_TargetIndex].rotation, Time.deltaTime * _LerpSpeedToPos[_TargetIndex]);
			_CameraComponent.fieldOfView = Mathf.Lerp(_CameraComponent.fieldOfView, _CameraComponentPoints[_TargetIndex].fieldOfView, Time.deltaTime * _LerpSpeedToPos[_TargetIndex]);
		}
	}

	private void SwitchToPosition()
	{
		_CameraToAnimateTRS.position = _CameraPositionsTRS[_TargetIndex].position;
		_CameraToAnimateTRS.rotation = _CameraPositionsTRS[_TargetIndex].rotation;
		_CameraComponent.fieldOfView = _CameraComponentPoints[_TargetIndex].fieldOfView;
	}

	private void NextPositionSwitch()
	{
		if (_TargetIndex < _CameraPositions.Length)
		{
			_TargetIndex++;
			_StartRotation = _CameraToAnimateTRS.rotation;
		}
		else
		{
			b_StartSequence = false;
		}
	}

	private IEnumerator PauseTime(float _Pausetime)
	{
		b_Paused = true;
		yield return new WaitForSeconds(_Pausetime);
		b_Paused = false;
		Debug.Log("Pause " + _Pausetime + " Time:" + Time.time);
	}

	private void ActivateFollowerCamera(bool _inBool)
	{
		if (_CameraComponent != null)
		{
			_CameraComponent.enabled = _inBool;
		}
	}
}

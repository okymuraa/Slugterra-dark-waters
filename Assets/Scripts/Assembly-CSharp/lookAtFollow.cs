using UnityEngine;

public class lookAtFollow : MonoBehaviour
{
	public GameObject _FollowTargetConnector;

	public GameObject _LookAtTarget;

	public float _FollowSpeed = 0.01f;

	public float _StopDistance = 0.01f;

	private Transform _FollowTargetConnectorTRS;

	private Transform _LookAtTargetTRS;

	private Transform _FollowerTRS;

	private void Start()
	{
		_FollowerTRS = transform;
		_FollowTargetConnectorTRS = _FollowTargetConnector.transform;
		_LookAtTargetTRS = _LookAtTarget.transform;
	}

	private void Update()
	{
		_FollowerTRS.LookAt(_LookAtTargetTRS.position);
		if ((_LookAtTargetTRS.position - _FollowTargetConnectorTRS.position).sqrMagnitude > _StopDistance)
		{
			_FollowerTRS.position = Vector3.Lerp(_FollowerTRS.position, _LookAtTargetTRS.position, _FollowSpeed);
		}
	}
}

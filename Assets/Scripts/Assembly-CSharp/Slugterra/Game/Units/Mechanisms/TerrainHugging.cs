using UnityEngine;

namespace Slugterra.Game.Units.Mechanisms
{
	internal class TerrainHugging : MonoBehaviour
	{
		[SerializeField]
		private Transform backLeft;

		[SerializeField]
		private Transform backRight;

		[SerializeField]
		private Transform frontLeft;

		[SerializeField]
		private Transform frontRight;

		private RaycastHit lr;

		private RaycastHit rr;

		private RaycastHit lf;

		private RaycastHit rf;

		private Transform _transform;

		[SerializeField]
		private float _updatingTimeDelay = 0.5f;

		private float _nextUpdate;

		private int _layerMask;

		private Vector3 _upDir;

		private void Start()
		{
			_transform = transform;
			int num = LayerMask.NameToLayer("Terrain");
			_layerMask = 1 << num;
		}

		private void Update()
		{
			if (_nextUpdate < Time.time)
			{
				Physics.Raycast(backLeft.position + Vector3.up, Vector3.down, out lr, float.PositiveInfinity, _layerMask);
				Physics.Raycast(backRight.position + Vector3.up, Vector3.down, out rr, float.PositiveInfinity, _layerMask);
				Physics.Raycast(frontLeft.position + Vector3.up, Vector3.down, out lf, float.PositiveInfinity, _layerMask);
				Physics.Raycast(frontRight.position + Vector3.up, Vector3.down, out rf, float.PositiveInfinity, _layerMask);
				_upDir = (Vector3.Cross(rr.point - Vector3.up, lr.point - Vector3.up) + Vector3.Cross(lr.point - Vector3.up, lf.point - Vector3.up) + Vector3.Cross(lf.point - Vector3.up, rf.point - Vector3.up) + Vector3.Cross(rf.point - Vector3.up, rr.point - Vector3.up)).normalized;
				Vector3 forward = Vector3.Cross(_upDir, _transform.parent.forward);
				Quaternion rotation = Quaternion.LookRotation(forward, _upDir);
				_transform.rotation = rotation;
				_nextUpdate += _updatingTimeDelay;
			}
		}
	}
}

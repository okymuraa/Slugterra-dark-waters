using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units
{
	public class DestroyUnitCommand : MonoCommand<DestroyUnitCommand>
	{
		private Transform _unitTransform;

		private float _destroyTime;

		protected override void OnStart(object[] args)
		{
			_unitTransform = (Transform)args[0];
			_destroyTime = Time.time + (float)args[1];
			GameObject original = Resources.Load("Prefabs/Game/Units/prf_FX_Soul_Up_Smoke") as GameObject;
			GameObject gameObject = Object.Instantiate(original, _unitTransform.position, Quaternion.identity);
		}

		private void Update()
		{
			if (_destroyTime <= Time.time)
			{
				if (_unitTransform != null)
				{
					Object.Destroy(_unitTransform.gameObject);
				}
				Finish();
			}
		}
	}
}

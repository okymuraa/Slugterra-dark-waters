using UnityEngine;

namespace Slugterra.Game.Mission
{
	public class DestroyObjectiveTarget : ObjectiveTarget
	{
		[SerializeField]
		private GameObject _target;

		protected override void OnCreate()
		{
			if (_target == null)
			{
				Debug.LogWarning("DestroyObjectiveTarget: target is null");
			}
			if (_target == gameObject)
			{
				Debug.LogWarning("DestroyObjectiveTarget: bad target");
			}
			if (_target != null)
			{
				transform.position = _target.transform.position;
			}
		}

		protected override void OnUpdate()
		{
			if (!Completed && _target == null)
			{
				Completed = true;
			}
		}
	}
}

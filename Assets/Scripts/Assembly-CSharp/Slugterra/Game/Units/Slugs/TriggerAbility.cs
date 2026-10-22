using AppsMinistry.Core;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class TriggerAbility : BaseMonoBehaviour
	{
		[SerializeField]
		private float _time;

		[SerializeField]
		private bool _destroyable = true;

		private float _destroyTime;

		private int _detectLayer;

		private SlugAbility[] _abilities;

		protected override void OnStart()
		{
			base.OnStart();
			_detectLayer = GetDetectLayer();
			_destroyTime = _time + Time.time;
			_abilities = GetComponents<SlugAbility>();
		}

		protected int GetDetectLayer()
		{
			if (GameObject.layer == LayerMask.NameToLayer("AllySlugLayer"))
			{
				return LayerMask.NameToLayer("EnemyLayer");
			}
			if (GameObject.layer == LayerMask.NameToLayer("EnemySlugLayer"))
			{
				return LayerMask.NameToLayer("AllyLayer");
			}
			return LayerMask.NameToLayer("EnemyLayer");
		}

		private void OnTriggerEnter(Collider collider)
		{
			if (collider.gameObject.layer == _detectLayer)
			{
				SlugAbility[] abilities = _abilities;
				foreach (SlugAbility slugAbility in abilities)
				{
					slugAbility.OnUse(collider.transform);
				}
			}
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (_destroyable && _destroyTime <= Time.time)
			{
				Object.Destroy(GameObject);
			}
		}
	}
}

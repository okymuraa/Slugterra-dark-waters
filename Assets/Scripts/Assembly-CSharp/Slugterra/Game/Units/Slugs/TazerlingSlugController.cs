using System;
using System.Collections.Generic;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	[Serializable]
	public class TazerlingSlugController : SlugController
	{
		[HideInInspector]
		[SerializeField]
		private List<int> _maxTargets;

		[HideInInspector]
		[SerializeField]
		private List<float> _damageDecays;

		private List<Transform> _hittedTarget;

		public List<int> MaxTargets
		{
			get
			{
				if (_maxTargets == null || _maxTargets.Count < 5)
				{
					InitMaxTargets();
				}
				return _maxTargets;
			}
		}

		public List<float> DamageDecays
		{
			get
			{
				if (_damageDecays == null || _damageDecays.Count < 5)
				{
					InitDamageDecays();
				}
				return _damageDecays;
			}
		}

		public override float Damage
		{
			get
			{
				float num = Mathf.Pow(_damageDecays[Model.Level], _hittedTarget.Count);
				return base.Damage * num;
			}
		}

		private void InitMaxTargets()
		{
			_maxTargets = new List<int>();
			for (int i = 0; i < 5; i++)
			{
				_maxTargets.Add(2);
			}
		}

		private void InitDamageDecays()
		{
			_damageDecays = new List<float>();
			for (int i = 0; i < 5; i++)
			{
				_damageDecays.Add(0.2f);
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			_hittedTarget = new List<Transform>();
		}

		protected override void OnSetSlugStrategy()
		{
			base.OnSetSlugStrategy();
			if (_strategy != null && _strategy is SlugStrategy)
			{
				(_strategy as SlugStrategy).MaxCollisionCount = _maxTargets[Model.Level];
			}
		}

		public override void CheckCollision(Collider collider)
		{
			if (!_hittedTarget.Contains(collider.transform) && _strategy.OnTriggerEnter(collider.transform))
			{
				_hittedTarget.Add(collider.transform);
				if (_hittedTarget.Count >= _maxTargets[Model.Level] || collider.gameObject.layer != GetLayer())
				{
					Target = null;
					SelfDestruction();
				}
				else if (!OnNextTarget())
				{
					Target = null;
					SelfDestruction();
				}
			}
		}

		private bool OnNextTarget()
		{
			Collider[] array = PhysicsFacade.OverlapSphere(Transform.position, Model.HittingArea, 1 << GetLayer());
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].gameObject.layer == GetLayer() && !_hittedTarget.Contains(array[i].transform))
				{
					Transform transform = array[i].transform;
					HitMarker component = transform.GetComponent<HitMarker>();
					Transform randomMarker = component.GetRandomMarker();
					Vector3 forward = randomMarker.position - Transform.position;
					Transform.rotation = Quaternion.LookRotation(forward);
					SetVelocity(forward.normalized);
					Target = randomMarker;
					return true;
				}
			}
			return false;
		}

		protected int GetLayer()
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
	}
}

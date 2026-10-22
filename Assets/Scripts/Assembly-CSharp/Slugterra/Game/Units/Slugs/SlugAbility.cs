using System.Collections.Generic;
using AppsMinistry.Core;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class SlugAbility : BaseMonoBehaviour
	{
		[SerializeField]
		private bool _isNegative = true;

		[SerializeField]
		private GameObject _particleEffect;

		[SerializeField]
		private bool _isSplash;

		protected bool _isCommon;

		protected SlugController Controller;

		protected int MaxUnitsForSplash = int.MaxValue;

		private int _splashedUnitsCounter;

		protected GameObject ParticleEffect
		{
			get
			{
				return _particleEffect;
			}
		}

		protected virtual float SplashArea
		{
			get
			{
				if (Controller == null || Controller.Model == null)
				{
					return 0f;
				}
				return Controller.Model.HittingArea;
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			Controller = GetComponent<SlugController>();
		}

		public void OnUse(Transform unitTransform)
		{
			if (_isSplash)
			{
				OnSplash();
			}
			else if (_isNegative)
			{
				UnitController component = unitTransform.GetComponent<UnitController>();
				if (component != null && !component.Model.Invulnerability)
				{
					OnUseNegativeEffect(unitTransform);
				}
			}
			else if (Controller.Parent != null)
			{
				OnUsePositiveEffect(Controller.Parent.Transform);
			}
		}

		protected virtual void OnUseNegativeEffect(Transform unitTransform)
		{
		}

		protected virtual void OnUseNegativeEffect(Transform[] unitTransform)
		{
		}

		protected virtual void OnUsePositiveEffect(Transform unitTransform)
		{
		}

		private void OnSplash()
		{
			if (_isNegative)
			{
				Collider[] array = PhysicsFacade.OverlapSphere(Transform.position, SplashArea, 1 << GetNegativeLayer());
				int num = 0;
				int num2 = LayerMask.NameToLayer("CoverLayer");
				num = 1 << num2;
				List<Transform> list = new List<Transform>();
				for (int i = 0; i < array.Length; i++)
				{
					if (_splashedUnitsCounter >= MaxUnitsForSplash)
					{
						break;
					}
					Transform transform = array[i].transform;
					UnitController component = transform.GetComponent<UnitController>();
					if (component == null || component.Model.Invulnerability || component.Model.AntiNegativeAbilities)
					{
						continue;
					}
					Vector3 position = Transform.position;
					Vector3 zero = Vector3.zero;
					HitMarker component2 = transform.GetComponent<HitMarker>();
					if (!(component2 != null))
					{
						continue;
					}
					foreach (Transform marker in component2.Markers)
					{
						zero = marker.position - position;
						float magnitude = (marker.position - position).magnitude;
						Ray ray = new Ray(position, zero);
						RaycastHit hitInfo;
						if (!Physics.Raycast(ray, out hitInfo, magnitude, num, QueryTriggerInteraction.Collide))
						{
							if (_isCommon)
							{
								list.Add(transform);
							}
							else
							{
								OnUseNegativeEffect(transform);
							}
							_splashedUnitsCounter++;
							break;
						}
					}
				}
				if (_isCommon)
				{
					OnUseNegativeEffect(list.ToArray());
				}
			}
			else if (Controller.Parent != null)
			{
				Collider[] array2 = PhysicsFacade.OverlapSphere(Controller.Parent.Transform.position, SplashArea, 1 << GetPositiveLayer());
				for (int j = 0; j < array2.Length; j++)
				{
					OnUsePositiveEffect(array2[j].transform);
				}
			}
		}

		protected int GetNegativeLayer()
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

		protected int GetPositiveLayer()
		{
			if (GameObject.layer == LayerMask.NameToLayer("AllySlugLayer"))
			{
				return LayerMask.NameToLayer("AllyLayer");
			}
			if (GameObject.layer == LayerMask.NameToLayer("EnemySlugLayer"))
			{
				return LayerMask.NameToLayer("EnemyLayer");
			}
			return LayerMask.NameToLayer("AllyLayer");
		}

		protected void Init<T>(ref List<T> _list)
		{
			_list = new List<T>();
			for (int i = 0; i < 5; i++)
			{
				_list.Add(default(T));
			}
		}
	}
}

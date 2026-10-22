using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Domain.Game.UI;
using UnityEngine;

namespace Slugterra.Game.Units.Slugs
{
	public class CrystalidSlugController : SlugController
	{
		[SerializeField]
		private bool _destroyAfterCollision;

		[HideInInspector]
		[SerializeField]
		private List<float> _distance;

		private Vector3 _startPosition;

		private Action UpdateStrategy;

		public List<float> Distance
		{
			get
			{
				if (_distance == null || _distance.Count < 5)
				{
					Init(ref _distance);
				}
				return _distance;
			}
		}

		protected void Init<T>(ref List<T> _list)
		{
			_list = new List<T>();
			for (int i = 0; i < 5; i++)
			{
				_list.Add(default(T));
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			_startPosition = Transform.position;
			if (MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Type == LevelType.Action)
			{
				UpdateStrategy = () =>
				{
					_003COnUpdate_003E__BaseCallProxy0();
					if (Vector3.SqrMagnitude(_startPosition - Transform.position) > Distance[Model.Level] * Distance[Model.Level])
					{
						OnExplosion();
						SelfDestruction();
					}
				};
			}
			else
			{
				UpdateStrategy = () =>
				{
					_003COnUpdate_003E__BaseCallProxy0();
				};
			}
		}

		protected override void OnSetSlugStrategy()
		{
			base.OnSetSlugStrategy();
			if (_strategy != null && _strategy is SlugStrategy)
			{
				(_strategy as SlugStrategy).MaxCollisionCount = int.MaxValue;
			}
		}

		protected override void OnUpdate()
		{
			UpdateStrategy();
		}

		public override void CheckCollision(Collider collider)
		{
			_strategy.OnTriggerEnter(collider.transform);
			if (!(_strategy is SlugStrategy))
			{
				return;
			}
			SlugStrategy slugStrategy = _strategy as SlugStrategy;
			if (slugStrategy.CollisionWithLevelItem(collider.transform))
			{
				IsAutoAttack = false;
				if (_destroyAfterCollision)
				{
					OnExplosion();
					SelfDestruction();
				}
			}
		}

		[CompilerGenerated]
		[DebuggerHidden]
		private void _003COnUpdate_003E__BaseCallProxy0()
		{
			base.OnUpdate();
		}
	}
}

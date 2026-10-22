using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace Slugterra.Game.Units.Player.Strategy
{
	public class BaseSplineInputStrategy : InputStrategy
	{
		public const float StartParam = 0.5f;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector2> OnRollEvent__BackingField;

		private Spline _spline;

		private float _param;

		private bool _initialize;

		public Spline CurrentSpline
		{
			get
			{
				return _spline;
			}
		}

		public float CurrentParam
		{
			get
			{
				return _param;
			}
			set
			{
				MoveTo(value);
			}
		}

		public event Action<Vector2> OnRollEvent
		{
			add
			{
				Action<Vector2> action = OnRollEvent__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRollEvent__BackingField, (Action<Vector2>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector2> action = OnRollEvent__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRollEvent__BackingField, (Action<Vector2>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public BaseSplineInputStrategy(PlayerController controller)
			: base(controller)
		{
		}

		protected virtual void OnInitialize()
		{
		}

		private void Initialize()
		{
			_spline = FindNearestSpline(Controller.transform.position);
			MoveTo(0.5f);
			OnInitialize();
		}

		protected Spline FindNearestSpline(Vector3 playerPosition)
		{
			Spline[] array = UnityEngine.Object.FindObjectsOfType<Spline>();
			Spline result = null;
			if (array != null && array.Length > 0)
			{
				float num = float.MaxValue;
				for (int i = 0; i < array.Length; i++)
				{
					float num2 = Vector3.Distance(playerPosition, array[i].transform.position);
					if (num > num2)
					{
						num = num2;
						result = array[i];
					}
				}
			}
			return result;
		}

		public override void OnUpdate()
		{
			if (!_initialize)
			{
				Initialize();
				_initialize = true;
			}
		}

		protected void OnRoll(Vector2 direction)
		{
			if (OnRollEvent__BackingField != null)
			{
				OnRollEvent__BackingField(direction);
			}
		}

		protected void MoveTo(float param)
		{
			if (_spline != null)
			{
				_param = Mathf.Clamp01(param);
				Controller.Transform.position = _spline.GetPositionOnSpline(_param);
				Controller.Transform.rotation = _spline.GetOrientationOnSpline(_param);
			}
		}
	}
}

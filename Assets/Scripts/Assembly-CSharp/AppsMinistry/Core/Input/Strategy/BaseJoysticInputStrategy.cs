using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Utils;
using Slugterra.Game.Units.NetPlayer;
using UnityEngine;

namespace AppsMinistry.Core.Input.Strategy
{
	public class BaseJoysticInputStrategy : IInputControllerStrategy
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector2> OnMoveEvent_movement__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnMoveEndEvent_movement__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector2> OnMoveEvent_rotation__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnMoveEndEvent_rotation__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Vector2, Vector2> OnRollEvent__BackingField;

		private bool _isMovement;

		private Vector2 _rollDir;

		private GameObject _playerGO;

		protected List<InputEvent> _enableEvents = new List<InputEvent>();

		public event Action<Vector2> OnMoveEvent_movement
		{
			add
			{
				Action<Vector2> action = OnMoveEvent_movement__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMoveEvent_movement__BackingField, (Action<Vector2>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector2> action = OnMoveEvent_movement__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMoveEvent_movement__BackingField, (Action<Vector2>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnMoveEndEvent_movement
		{
			add
			{
				Action action = OnMoveEndEvent_movement__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMoveEndEvent_movement__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnMoveEndEvent_movement__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMoveEndEvent_movement__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector2> OnMoveEvent_rotation
		{
			add
			{
				Action<Vector2> action = OnMoveEvent_rotation__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMoveEvent_rotation__BackingField, (Action<Vector2>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector2> action = OnMoveEvent_rotation__BackingField;
				Action<Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMoveEvent_rotation__BackingField, (Action<Vector2>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnMoveEndEvent_rotation
		{
			add
			{
				Action action = OnMoveEndEvent_rotation__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMoveEndEvent_rotation__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnMoveEndEvent_rotation__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMoveEndEvent_rotation__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Vector2, Vector2> OnRollEvent
		{
			add
			{
				Action<Vector2, Vector2> action = OnRollEvent__BackingField;
				Action<Vector2, Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRollEvent__BackingField, (Action<Vector2, Vector2>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Vector2, Vector2> action = OnRollEvent__BackingField;
				Action<Vector2, Vector2> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRollEvent__BackingField, (Action<Vector2, Vector2>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public virtual void Activate()
		{
		}

		public virtual void Deactivate()
		{
		}

		public virtual void Update()
		{
		}

		public void EnableEvent(InputEvent inputEvent)
		{
			if (!_enableEvents.Contains(inputEvent))
			{
				_enableEvents.Add(inputEvent);
			}
		}

		public void DisableEvent(InputEvent inputEvent)
		{
			if (_enableEvents.Contains(inputEvent))
			{
				_enableEvents.Remove(inputEvent);
			}
		}

		public bool IsEventEnabled(InputEvent inputEvent)
		{
			return _enableEvents.Contains(inputEvent);
		}

		protected void Use_OnMoveEvent_movement(Vector2 axies)
		{
			_isMovement = true;
			_rollDir = axies;
			if (IsEventEnabled(InputEvent.OnMove) && OnMoveEvent_movement__BackingField != null)
			{
				OnMoveEvent_movement__BackingField(axies);
			}
		}

		protected void Use_OnMoveEndEvent_movement()
		{
			_isMovement = false;
			_rollDir = new Vector2(0f, 0f);
			if (IsEventEnabled(InputEvent.OnMoveEnd) && OnMoveEndEvent_movement__BackingField != null)
			{
				OnMoveEndEvent_movement__BackingField();
			}
		}

		protected void Use_OnMoveEvent_rotation(Vector2 axies)
		{
			if (IsEventEnabled(InputEvent.OnMove) && OnMoveEvent_rotation__BackingField != null)
			{
				OnMoveEvent_rotation__BackingField(axies);
			}
		}

		protected void Use_OnMoveEndEvent_rotation()
		{
			if (IsEventEnabled(InputEvent.OnMoveEnd) && OnMoveEndEvent_rotation__BackingField != null)
			{
				OnMoveEndEvent_rotation__BackingField();
			}
		}

		protected void Use_OnRollEvent()
		{
			if (_playerGO == null)
			{
				PhotonView[] array = UnityEngine.Object.FindObjectsOfType<PhotonView>();
				PhotonView[] array2 = array;
				foreach (PhotonView photonView in array2)
				{
					if (photonView.isMine)
					{
						_playerGO = photonView.gameObject;
						return;
					}
				}
			}
			float num = 0f;
			float num2 = 0f;
			Vector2 vector = new Vector2(0f, 0f);
			if (_playerGO != null)
			{
				NetPlayerController component = _playerGO.GetComponent<NetPlayerController>();
				num = component.Animator.transform.localEulerAngles.y + component.Transform.localEulerAngles.y;
				num2 = _playerGO.transform.eulerAngles.y;
			}
			if (_isMovement)
			{
				vector = MathUtil.Rotate(_rollDir, 0f - num2);
				_rollDir = MathUtil.Rotate(_rollDir, 0f - num);
			}
			else
			{
				_rollDir = MathUtil.Rotate(Vector2.up, 0f - num);
				vector = MathUtil.Rotate(Vector2.up, 0f - num2);
			}
			if (OnRollEvent__BackingField != null)
			{
				OnRollEvent__BackingField(_rollDir, vector);
			}
		}
	}
}

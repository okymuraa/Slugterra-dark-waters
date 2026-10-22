using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace Slugterra.Game.Triggers
{
	[RequireComponent(typeof(Collider))]
	internal class OnTriggerEventsSender : MonoBehaviour
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Collider> OnTriggerEnterEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Collider> OnTriggerExitEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<Collider> OnTriggerEnterStay__BackingField;

		public Collider Trigger { get; private set; }

		public event Action<Collider> OnTriggerEnterEvent
		{
			add
			{
				Action<Collider> action = OnTriggerEnterEvent__BackingField;
				Action<Collider> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTriggerEnterEvent__BackingField, (Action<Collider>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Collider> action = OnTriggerEnterEvent__BackingField;
				Action<Collider> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTriggerEnterEvent__BackingField, (Action<Collider>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Collider> OnTriggerExitEvent
		{
			add
			{
				Action<Collider> action = OnTriggerExitEvent__BackingField;
				Action<Collider> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTriggerExitEvent__BackingField, (Action<Collider>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Collider> action = OnTriggerExitEvent__BackingField;
				Action<Collider> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTriggerExitEvent__BackingField, (Action<Collider>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<Collider> OnTriggerEnterStay
		{
			add
			{
				Action<Collider> action = OnTriggerEnterStay__BackingField;
				Action<Collider> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTriggerEnterStay__BackingField, (Action<Collider>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<Collider> action = OnTriggerEnterStay__BackingField;
				Action<Collider> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTriggerEnterStay__BackingField, (Action<Collider>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private void Awake()
		{
			Trigger = GetComponent<Collider>();
			Trigger.isTrigger = true;
		}

		private void OnTriggerEnter(Collider collider)
		{
			if (OnTriggerEnterEvent__BackingField != null)
			{
				OnTriggerEnterEvent__BackingField(collider);
			}
		}

		private void OnTriggerExit(Collider collider)
		{
			if (OnTriggerExitEvent__BackingField != null)
			{
				OnTriggerExitEvent__BackingField(collider);
			}
		}

		private void OnTriggerStay(Collider collider)
		{
			if (OnTriggerEnterStay__BackingField != null)
			{
				OnTriggerEnterStay__BackingField(collider);
			}
		}
	}
}

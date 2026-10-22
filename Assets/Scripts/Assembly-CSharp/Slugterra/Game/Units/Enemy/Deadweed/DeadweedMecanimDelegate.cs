using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace Slugterra.Game.Units.Enemy.Deadweed
{
	public class DeadweedMecanimDelegate : MonoBehaviour
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnAttackEvent__BackingField;

		public event Action OnAttackEvent
		{
			add
			{
				Action action = OnAttackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAttackEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnAttackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnAttackEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private void OnAttack()
		{
			if (OnAttackEvent__BackingField != null)
			{
				OnAttackEvent__BackingField();
			}
		}
	}
}

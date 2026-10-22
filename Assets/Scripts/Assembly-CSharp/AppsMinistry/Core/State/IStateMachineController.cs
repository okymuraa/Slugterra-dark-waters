using UnityEngine;

namespace AppsMinistry.Core.State
{
	public interface IStateMachineController
	{
		GameObject GameObject { get; }

		void Next(MonoState prevState);
	}
}

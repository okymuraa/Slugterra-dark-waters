using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units
{
	public class WaitCommand : MonoCommand<WaitCommand>
	{
		private float _waitTime;

		protected override void OnStart(object[] args)
		{
			float num = (float)args[0];
			_waitTime = num + Time.time;
		}

		private void Update()
		{
			if (_waitTime < Time.time)
			{
				Finish();
			}
		}
	}
}

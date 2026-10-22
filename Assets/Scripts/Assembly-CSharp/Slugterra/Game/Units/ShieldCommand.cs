using AppsMinistry.Core.Command;
using Slugterra.Domain.Game.Units;
using UnityEngine;

namespace Slugterra.Game.Units
{
	[SingletonCommand]
	public class ShieldCommand : MonoCommand<ShieldCommand>
	{
		private float _shieldDeactivateTime;

		private Unit _model;

		protected override void OnStart(object[] args)
		{
			base.OnStart(args);
			_model = args[0] as Unit;
			float num = (float)args[1];
			_shieldDeactivateTime = Time.time + num;
			_model.Invulnerability = true;
		}

		private void Update()
		{
			if (_shieldDeactivateTime < Time.time)
			{
				_model.Invulnerability = false;
				Finish();
			}
		}
	}
}

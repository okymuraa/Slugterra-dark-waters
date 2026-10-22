using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units.Ally.Assistant
{
	[SingletonCommand]
	public class LinkCommand : MonoCommand<LinkCommand>
	{
		private Transform _linkObject;

		protected override void OnStart(object[] args)
		{
			_linkObject = (Transform)args[0];
		}

		private void Update()
		{
			if (_linkObject == null)
			{
				Finish();
			}
		}
	}
}

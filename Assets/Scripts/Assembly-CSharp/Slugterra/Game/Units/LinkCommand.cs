using AppsMinistry.Core.Command;
using UnityEngine;

namespace Slugterra.Game.Units
{
	[SingletonCommand]
	public class LinkCommand : MonoCommand<LinkCommand>
	{
		private Transform _linkObject;

		private Vector3 _offset;

		private Transform _cachedTransform;

		protected override void OnStart(object[] args)
		{
			_linkObject = (Transform)args[0];
			if (args.Length == 1)
			{
				_offset = Vector3.zero;
			}
			else
			{
				_offset = (Vector3)args[1];
			}
			_cachedTransform = transform;
		}

		private void Update()
		{
			if (_linkObject == null || !_linkObject.gameObject.activeSelf)
			{
				Finish();
			}
			else
			{
				_cachedTransform.position = _linkObject.position + _offset;
			}
		}
	}
}

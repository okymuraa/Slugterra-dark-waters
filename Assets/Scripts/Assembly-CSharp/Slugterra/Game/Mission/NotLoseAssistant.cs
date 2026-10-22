using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.Units;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.Game.Mission
{
	internal class NotLoseAssistant : ObjectiveTarget, IObserver
	{
		private GameController _gameController;

		private bool _fail;

		protected override void OnStart()
		{
			base.OnStart();
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (missionSetting.Assistant.Type == AllyType.None)
			{
				_fail = true;
				Completed = false;
			}
			else
			{
				_gameController.Model.Assistant.AddObserver(this);
			}
		}

		private void OnEnable()
		{
			_gameController = Object.FindObjectOfType<GameController>();
			if (_gameController != null)
			{
				_gameController.OnCompleteMissionEvent += OnComplete;
			}
		}

		private void OnDisable()
		{
			if (_gameController != null)
			{
				_gameController.OnCompleteMissionEvent -= OnComplete;
			}
		}

		private void OnComplete()
		{
			if (!_fail)
			{
				Completed = true;
			}
		}

		public new void OnObjectChanged(object observable)
		{
			if (observable is Humanoid)
			{
				Humanoid humanoid = observable as Humanoid;
				if (humanoid.IsDead())
				{
					_fail = true;
					Completed = false;
					humanoid.RemoveObserver(this);
				}
			}
		}
	}
}

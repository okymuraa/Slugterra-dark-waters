using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class ResultMultiplayerDrawView : ResultMultiplayerView
	{
		private GameController _gameController;

		protected override void OnEnable()
		{
			base.OnEnable();
			_btns.SetActiveObjects();
			if (_gameController == null)
			{
				_gameController = Object.FindObjectOfType<GameController>();
			}
			if (_gameController != null && _gameController.Model.GameFailed)
			{
				_statisticController.View.GetTotalRewards(MonoSingleton<AppController>.Instance.GetCurrentGameLevel(), false);
			}
			_btnNext.targetObject = gameObject;
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			_btns.SetDisableObjects();
		}
	}
}

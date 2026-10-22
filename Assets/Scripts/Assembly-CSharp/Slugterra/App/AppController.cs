using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Domain.App;
using Slugterra.Domain.Game.UI;
using Slugterra.Scene;
using UnityEngine;

namespace Slugterra.App
{
	[AppSingleton]
	public class AppController : MonoSingleton<AppController>
	{
		private AppModel _model;

		private float _currentTimeScale;

		private bool _isLoaded;

		public AppModel Model
		{
			get
			{
				return _model;
			}
			set
			{
				_model = value;
			}
		}

		public float TimeScale
		{
			get
			{
				return _currentTimeScale;
			}
			set
			{
				_currentTimeScale = value;
				Time.timeScale = _currentTimeScale;
			}
		}

		protected override void OnCreate()
		{
			_currentTimeScale = 1f;
			Screen.fullScreen = true;
			_model = new AppModel();
			Load();
		}

		public void Load()
		{
			if (!_isLoaded)
			{
				MonoSingleton<AppConfiguration>.Instance.Load();
				MonoSingleton<GameConfiguration>.Instance.Load();
				_model.Load();
				Model.ShopModel.CheckSlugs();
				Model.ShopModel.CheckArmor();
				_isLoaded = true;
			}
		}

		protected override void OnQuit()
		{
			_model.Save();
		}

		public LevelItem GetCurrentGameLevel()
		{
			SlugterraSceneController slugterraSceneController = Object.FindObjectOfType<SlugterraSceneController>();
			if (slugterraSceneController != null)
			{
				return Model.GetLevelItem(slugterraSceneController.GetType());
			}
			return Model.GetLevelItem(Model.MissionSetting.Mission);
		}
	}
}

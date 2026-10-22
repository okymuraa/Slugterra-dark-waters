using AppsMinistry.Core;
using AppsMinistry.Domain;
using AppsMinistry.Domain.Observer;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Scene;
using Slugterra.Scene.Game;

namespace Slugterra.Domain
{
	public class GameModel : DomainObject, IObserver
	{
		private GameState _state;

		private bool _gameFailed;

		private bool _gameWon;

		private Humanoid _eli;

		private Humanoid _p1Eli;

		private Humanoid _assistant;

		private Humanoid _boss;

		private int _respawnCount;

		public GameState State
		{
			get
			{
				return _state;
			}
			set
			{
				if (_state != value)
				{
					_state = value;
					CommitAndNotify();
				}
			}
		}

		public bool GameFailed
		{
			get
			{
				return _gameFailed;
			}
			set
			{
				if (_gameFailed != value)
				{
					_gameFailed = value;
					CommitAndNotify();
				}
			}
		}

		public bool GameWon
		{
			get
			{
				return _gameWon;
			}
			set
			{
				if (_gameWon != value)
				{
					_gameWon = value;
					CommitAndNotify();
				}
			}
		}

		public Humanoid Eli
		{
			get
			{
				return _eli;
			}
			set
			{
				if (_eli != null)
				{
					_eli.RemoveObserver(this);
				}
				_eli = value;
				_eli.AddObserver(this);
			}
		}

		public Humanoid P1Eli
		{
			get
			{
				return _p1Eli;
			}
			set
			{
				if (_p1Eli != null)
				{
					_p1Eli.RemoveObserver(this);
				}
				_p1Eli = value;
				_p1Eli.AddObserver(this);
			}
		}

		public Humanoid Assistant
		{
			get
			{
				return _assistant;
			}
			set
			{
				_assistant = value;
				CommitAndNotify();
			}
		}

		public Humanoid Boss
		{
			get
			{
				return _boss;
			}
			set
			{
				if (_boss != null)
				{
					_boss.RemoveObserver(this);
				}
				_boss = value;
				_boss.AddObserver(this);
				CommitAndNotify();
			}
		}

		public int RespawnCount
		{
			get
			{
				return _respawnCount;
			}
			set
			{
				_respawnCount = value;
				CommitAndNotify();
			}
		}

		public static GameModel CreateGameModel(HumanoidLevels eliConfig)
		{
			GameModel gameModel = new GameModel();
			gameModel.Eli = new Humanoid(eliConfig);
			gameModel.P1Eli = new Humanoid(eliConfig);
			return gameModel;
		}

		public static GameModel CreateGameModel(HumanoidLevels eliConfig, MissionSetting missionSettings)
		{
			GameModel gameModel = CreateGameModel(eliConfig);
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			if (currentGameLevel.Scene == typeof(ScorchedSparksTutorial_1_1))
			{
				gameModel.Eli.AddSlug(new Slug(SlugType.Infurnus));
			}
			else
			{
				if (missionSettings.AttackSlug != null && missionSettings.AttackSlug.Type != SlugType.None)
				{
					gameModel.Eli.AddSlug(new Slug(missionSettings.AttackSlug.Type, missionSettings.AttackSlug.Level));
				}
				if (missionSettings.DefenceSlug != null && missionSettings.DefenceSlug.Type != SlugType.None)
				{
					gameModel.Eli.AddSlug(new Slug(missionSettings.DefenceSlug.Type, missionSettings.DefenceSlug.Level));
				}
				if (missionSettings.SpecialSlug != null && missionSettings.SpecialSlug.Type != SlugType.None)
				{
					gameModel.Eli.AddSlug(new Slug(missionSettings.SpecialSlug.Type, missionSettings.SpecialSlug.Level));
				}
			}
			if (currentGameLevel.Scene == typeof(ShaneTutor_0_1) || currentGameLevel.Scene == typeof(ScorchedSparksTutorial_1_1) || currentGameLevel.Scene == typeof(QuietLawn_2_1) || currentGameLevel.Scene == typeof(QuietLawn_2_2))
			{
				gameModel.Eli.SetBlaster(MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(BlasterType.LeaveBlaster, 0), BlasterType.LeaveBlaster);
			}
			else if (currentGameLevel.Scene == typeof(ShopSceneController) || currentGameLevel.Scene == typeof(BackpackSceneController) || currentGameLevel.Scene == typeof(MultiplayerSceneController))
			{
				gameModel.Eli.SetBlaster(MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(BlasterType.LeaveBlaster, 6), BlasterType.LeaveBlaster);
			}
			else
			{
				gameModel.Eli.SetBlaster(MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(missionSettings.Blaster.Type, missionSettings.Blaster.Level), missionSettings.Blaster.Type);
				gameModel.P1Eli.SetBlaster(MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlasterParams(missionSettings.Blaster.Type, missionSettings.Blaster.Level), missionSettings.Blaster.Type);
			}
			gameModel.Eli.SetArmor(MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorParams(missionSettings.Armor.Type, missionSettings.Armor.Level));
			return gameModel;
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is Humanoid && observable.Equals(_eli))
			{
				OnEliChanged(observable as Humanoid);
			}
		}

		private void OnEliChanged(Humanoid eliModel)
		{
		}

		private void OnEliAssistChanged(Humanoid eliAssistModel)
		{
		}
	}
}

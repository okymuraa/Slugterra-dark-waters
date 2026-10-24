using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.Domain.Observer;
using AppsMinistry.Plugins;
using AppsMinistry.Scene;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using HedgehogTeam.EasyTouch;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain;
using Slugterra.Domain.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.Mission;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.Game.Units;
using Slugterra.Domain.Game.Units.Slugs;
using Slugterra.Domain.Shops;
using Slugterra.Domain.UI.Game;
using Slugterra.Game;
using Slugterra.Game.Duel;
using Slugterra.Game.Mission;
using Slugterra.Game.Units.Ally.Assistant;
using Slugterra.Game.Units.Enemy;
using Slugterra.Game.Units.NetPlayer;
using Slugterra.Game.Units.Player;
using Slugterra.Scene;
using Slugterra.Scene.Daily;
using Slugterra.Scene.Game;
using Slugterra.Scene.Multiplayer;
using Slugterra.UI.Game.SlugFight;
using Slugterra.UI.Shops;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class GameController : UIController<GameView>, IObserver
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<bool> OnPlayCutSceneEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnCompleteMissionEvent__BackingField;

		private bool _gameFailed;

		private bool _gameWon;

		private bool _isMultiplayer;

		[SerializeField]
		private LevelType _levelType = LevelType.Action;

		private GameModel _model;

		private InGameController _inGameController;

		private bool _isPlayCutScene;

		[SerializeField]
		private Transform _assistantSpawnPoint;

		private SlugterraSceneController _sceneController;

		private bool result;

		public bool IsMultiplayer
		{
			get
			{
				return _isMultiplayer;
			}
		}

		public LevelType LevelType
		{
			get
			{
				return _levelType;
			}
		}

		public GameModel Model
		{
			get
			{
				return _model;
			}
		}

		public InGameController InGameController
		{
			get
			{
				return _inGameController;
			}
		}

		public bool IsPlayCutScene
		{
			get
			{
				return _isPlayCutScene;
			}
		}

		public Transform AssistantSpawnPoint
		{
			get
			{
				return _assistantSpawnPoint;
			}
		}

		private SlugterraSceneController SceneController
		{
			get
			{
				if (_sceneController == null)
				{
					_sceneController = UnityEngine.Object.FindObjectOfType<SlugterraSceneController>();
				}
				return _sceneController;
			}
		}

		public event Action<bool> OnPlayCutSceneEvent
		{
			add
			{
				Action<bool> action = OnPlayCutSceneEvent__BackingField;
				Action<bool> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayCutSceneEvent__BackingField, (Action<bool>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<bool> action = OnPlayCutSceneEvent__BackingField;
				Action<bool> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayCutSceneEvent__BackingField, (Action<bool>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnCompleteMissionEvent
		{
			add
			{
				Action action = OnCompleteMissionEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCompleteMissionEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnCompleteMissionEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCompleteMissionEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnCreate()
		{
			DonDestroyMonoSingleton<InputManager>.Instance.RegisterIgnoreLayer(LayerMask.NameToLayer("UI"));
			AppModel model = MonoSingleton<AppController>.Instance.Model;
			SlugterraSceneController sceneController = SceneController;
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			_isMultiplayer = currentGameLevel.Name.Contains("multiplayer") || currentGameLevel.Scene == typeof(DesertBase1Multiplayer);
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			DuelSetting duelSetting = MonoSingleton<AppController>.Instance.Model.DuelSetting;
			if (sceneController != null)
			{
				_levelType = model.GetLevelItem(sceneController.GetType()).Type;
			}
			else
			{
				_levelType = model.GetLevelItem(model.MissionSetting.Mission).Type;
			}
			if (_levelType == LevelType.Action)
			{
				_model = GameModel.CreateGameModel(MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfigurationLevel(AllyType.Eli, 0), model.MissionSetting);
			}
			else if (_levelType == LevelType.Arena)
			{
				_model = GameModel.CreateGameModel(MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfigurationLevel(AllyType.Eli, 0), model.MissionSetting);
				duelSetting = MonoSingleton<AppController>.Instance.Model.DuelSetting;
			}
			InGameModel inGameModel = new InGameModel(_levelType);
			inGameModel.Eli = _model.Eli;
			inGameModel.Assistant = _model.Assistant;
			EasyTouch.SetEnableAutoSelect(false);
			EasyTouch.Set3DPickableLayer(LayerMask.NameToLayer("Nothing"));
			if (model.ShopModel.Consumables != null)
			{
				inGameModel.ConsumableItems.AddRange(model.ShopModel.Consumables);
			}
			if (GetLevelItem().Scene == typeof(ScorchedSparksTutorial_1_1))
			{
				inGameModel.AttackSlug = Model.Eli.GetSlug(SlugType.Infurnus);
			}
			else if (model.MissionSetting.AttackSlug != null)
			{
				inGameModel.AttackSlug = _model.Eli.GetSlug(model.MissionSetting.AttackSlug.Type);
			}
			if (model.MissionSetting.DefenceSlug != null)
			{
				inGameModel.DefenceSlug = _model.Eli.GetSlug(model.MissionSetting.DefenceSlug.Type);
			}
			if (model.MissionSetting.SpecialSlug != null)
			{
				inGameModel.SpecialSlug = _model.Eli.GetSlug(model.MissionSetting.SpecialSlug.Type);
			}
			if (_levelType == LevelType.Action)
			{
				AssistantController assistantController = UnityEngine.Object.FindObjectOfType<AssistantController>();
				if (_assistantSpawnPoint != null && assistantController == null)
				{
					missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
					if (missionSetting.Assistant.Type != AllyType.None)
					{
						GameObject assistant = GameFactory.GetAssistant(missionSetting.Assistant.Type);
						GameObject gameObject = UnityEngine.Object.Instantiate(assistant);
						gameObject.transform.parent = _assistantSpawnPoint.parent;
						gameObject.transform.position = _assistantSpawnPoint.transform.position;
					}
				}
			}
			else if (_levelType == LevelType.Arena)
			{
				BossController bossController = UnityEngine.Object.FindObjectOfType<BossController>();
				if (_assistantSpawnPoint != null && bossController == null)
				{
					duelSetting = MonoSingleton<AppController>.Instance.Model.DuelSetting;
					GameObject foe = GameFactory.GetFoe(EnemyType.BossKord.ToString());
					if (currentGameLevel.Scene != typeof(ScorchedSparksTutorial_1_1) && duelSetting.Foe != null && !string.IsNullOrEmpty(duelSetting.Foe.Type) && duelSetting.Foe.Type != EnemyType.None.ToString())
					{
						GameObject customFoe = GameFactory.GetFoe(duelSetting.Foe.Type);
						if (customFoe != null)
						{
							foe = customFoe;
						}
					}
					GameObject gameObject2 = UnityEngine.Object.Instantiate(foe);
					bossController = gameObject2.GetComponentInChildren<BossController>();
					if (currentGameLevel.Scene == typeof(ScorchedSparksTutorial_1_1))
					{
						bossController.ReloadSlugMagazine();
						bossController.ClearAvailableSlug();
						bossController.AddSlug(new Slug(SlugType.Rammstone));
					}
					inGameModel.Boss = (Humanoid)bossController.Model;
					gameObject2.transform.parent = _assistantSpawnPoint.parent;
					gameObject2.transform.position = _assistantSpawnPoint.transform.position;
				}
			}
			_inGameController = UnityEngine.Object.FindObjectOfType<InGameController>();
			_inGameController.OnApplyModel(inGameModel);
			_model.AddObserver(this);
		}

		protected override void OnStart()
		{
			View.ApplyModel(_model);
			if (_levelType == LevelType.Arena)
			{
				bool flag = false;
				Attribute[] customAttributes = Attribute.GetCustomAttributes(SceneController.GetType());
				foreach (Attribute attribute in customAttributes)
				{
					if (attribute is SceneNameAttribute)
					{
						flag = (attribute as SceneNameAttribute).IsMultiplayer;
						break;
					}
				}
				if (flag)
				{
					PlayerManager.OnDuel((PlayerController local, PlayerController enemy) =>
					{
						MonoSingleton<DuelManager>.Instance.Initialize(local, enemy);
					});
				}
				else
				{
					PlayerManager.OnAddLocalPlayer((PlayerController player) =>
					{
						MonoSingleton<DuelManager>.Instance.Initialize(player, UnityEngine.Object.FindObjectOfType<BossController>());
					});
				}
			}
			DonDestroyMonoSingleton<AnalyticsManager>.Instance.EnterOnLevel(SceneController.GetType().Name);
			DonDestroyMonoSingleton<AnalyticsManager>.Instance.StartLevel();
		}

		protected override void _OnEnable()
		{
			base._OnEnable();
			View.OnPlayEvent += OnPlay;
			View.OnPauseEvent += OnPause;
			DonDestroyMonoSingleton<InputManager>.Instance.PauseEvent += OnPause;
			View.OnRespawnEvent += OnRespawn;
			View.OnMinimapEvent += OnMinimap;
			View.OnSkipMissionEvent += SkipMission;
			DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent += OnBack;
			Invoke("StartRecord", 3f);
		}

		private void StartRecord()
		{
		}

		protected override void _OnDisable()
		{
			base._OnDisable();
			View.OnPlayEvent -= OnPlay;
			View.OnPauseEvent -= OnPause;
			View.OnRespawnEvent -= OnRespawn;
			View.OnMinimapEvent -= OnMinimap;
			View.OnSkipMissionEvent -= SkipMission;
			if (DonDestroyMonoSingleton<InputManager>.IsAlive)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.PauseEvent -= OnPause;
			}
			DonDestroyMonoSingleton<InputManager>.Instance.BackBtnEvent -= OnBack;
		}

		private void OnPlay()
		{
			_model.State = GameState.Play;
		}

		private void OnPause()
		{
			if (SceneController.GetType() != typeof(ShopSceneController) && SceneController.GetType() != typeof(BackpackSceneController) && SceneController.GetType() != typeof(MultiplayerSceneController) && !_model.GameFailed && !_model.GameWon && _model.State != GameState.CutScene)
			{
				_model.State = GameState.Pause;
			}
		}

		private IEnumerator UpdateViews()
		{
			yield return StartCoroutine(CollectionUtils.WaitForRealSeconds(0.5f));
			UnityEngine.Debug.Log("UpdateViews");
		}

		private void OnMinimap()
		{
			_model.State = GameState.Minimap;
		}

		public void OnRespawn()
		{
			float num = GameFactory.GetRespawnCost(Model.RespawnCount);
			Slugterra.Domain.App.Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			if (wallet.Gold >= num)
			{
				RespawnPlayer();
				wallet.Gold -= num;
				return;
			}
			ShopIAPView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<ShopIAPView>();
			if (registeredPopup != null)
			{
				registeredPopup.Show();
			}
		}

		public void RespawnPlayerForAds()
		{
			if (PlayerManager.RespawnLocalPlayer(Model) != null)
			{
				_gameFailed = false;
				OnPlay();
			}
		}

		public void RespawnPlayer()
		{
			if (PlayerManager.RespawnLocalPlayer(Model) != null)
			{
				_gameFailed = false;
				OnPlay();
			}
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is GameModel)
			{
				OnGameModelChanged(observable as GameModel);
			}
		}

		private void OnGameModelChanged(GameModel model)
		{
			if (MonoSingleton<AppController>.Instance != null)
			{
				PlayerController localPlayer = PlayerManager.LocalPlayer;
				if ((model.State == GameState.Play || model.State == GameState.CutScene) && !(localPlayer is NetPlayerController))
				{
					Time.timeScale = MonoSingleton<AppController>.Instance.TimeScale;
					if (localPlayer != null)
					{
						localPlayer.enabled = true;
					}
				}
				else if (!(localPlayer is NetPlayerController))
				{
					Time.timeScale = 0f;
					if (localPlayer != null)
					{
						localPlayer.enabled = false;
					}
				}
			}
			if (model.Assistant != _inGameController.Model.Assistant)
			{
				_inGameController.Model.Assistant = model.Assistant;
			}
			if (model.Boss != _inGameController.Model.Boss)
			{
				_inGameController.Model.Boss = model.Boss;
			}
			if (Model.GameFailed && !_gameFailed)
			{
				_gameFailed = true;
				StartCoroutine(GameFailed(2f));
			}
			if (Model.GameWon && !_gameWon)
			{
				if (OnCompleteMissionEvent__BackingField != null)
				{
					OnCompleteMissionEvent__BackingField();
				}
				_gameWon = true;
				LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
				if (currentGameLevel != null)
				{
					if (currentGameLevel.Type == LevelType.Action)
					{
						OnMissionComplete(currentGameLevel);
					}
					else if (currentGameLevel.Type == LevelType.Arena)
					{
						StartCoroutine(OnArenaComplete(currentGameLevel, 3f));
					}
				}
			}
			View.ApplyModel(model);
		}

		private IEnumerator GameFailed(float time)
		{
			yield return new WaitForSeconds(time);
			if (_isMultiplayer)
			{
				Slugterra.Domain.App.Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
				LevelProperties levelProperties = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelProperties(MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene.Name);
				levelProperties.realMainObjectiveFailGoldReward = levelProperties.mainObjectiveFailRewardGold;
				levelProperties.realMainObjectiveFailGemsReward = levelProperties.mainObjectiveFailRewardGems;
				UnityEngine.Debug.Log("BONUS TO FAIL");
				wallet.Gold += levelProperties.realMainObjectiveFailGoldReward;
				wallet.Gems += levelProperties.realMainObjectiveFailGemsReward;
			}
			if (MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene != typeof(DesertBase_1) && MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene != typeof(DesertBase_2) && MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene != typeof(EastSurvival_1))
			{
				_model.State = GameState.Defeat;
			}
			else
			{
				_model.State = GameState.Survival;
			}
		}

		private IEnumerator GameWin(float time)
		{
			yield return new WaitForSeconds(time);
			if (MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene != typeof(DesertBase_1) && MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene != typeof(DesertBase_2) && MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene != typeof(EastSurvival_1))
			{
				_model.State = GameState.Complete;
			}
			else
			{
				_model.State = GameState.Survival;
			}
		}

		private LevelItem GetLevelItem()
		{
			AppModel model = MonoSingleton<AppController>.Instance.Model;
			SlugterraSceneController sceneController = SceneController;
			if (sceneController != null)
			{
				return model.GetLevelItem(sceneController.GetType());
			}
			return model.GetLevelItem(MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission);
		}

		private IEnumerator OnArenaComplete(LevelItem selectedLevel, float time)
		{
			yield return new WaitForSeconds(time);
			OnMissionComplete(selectedLevel);
		}

		private void OnMissionComplete(LevelItem selectedLevel)
		{
			UpdateProgress(selectedLevel);
			if (_isMultiplayer)
			{
				TakeRewardMultiplayer(selectedLevel);
			}
			else
			{
				TakeReward(selectedLevel);
			}
			_model.State = GameState.Complete;
			if (selectedLevel.Scene == typeof(QuietLawn_2_5) && !PlayerPrefsSystem.HasKey("enable_daily_missions"))
			{
				PlayerPrefs.SetInt("enable_daily_missions", 1);
				if (!DailySceneManager.IsCurrentDailyMissionComplete())
				{
					PlayerPrefs.SetInt("daily_missions_available", 1);
					DailySceneManager.SendPushForFirstDailyMission();
				}
			}
			if (DailySceneManager.IsCurrentDailyMissionComplete())
			{
				PlayerPrefs.SetInt("daily_missions_available", 0);
			}
			PlayerPrefs.Save();
		}

		private void TakeRewardMultiplayer(LevelItem selectedLevel)
		{
			Slugterra.Domain.App.Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			LevelProperties levelProperties = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelProperties(selectedLevel.Scene.Name);
			levelProperties.realMainObjectiveGoldReward = levelProperties.mainObjectiveRewardGold;
			levelProperties.realMainObjectiveGemsReward = levelProperties.mainObjectiveRewardGems;
			wallet.Stars++;
			UnityEngine.Debug.Log("GameController::TakeReward Multiplier: " + MonoSingleton<AppController>.Instance.Model.ShopModel.AvailableMultiplier);
			wallet.Gold += levelProperties.realMainObjectiveGoldReward * (float)MonoSingleton<AppController>.Instance.Model.ShopModel.AvailableMultiplier;
			wallet.Gems += levelProperties.realMainObjectiveGemsReward * (float)MonoSingleton<AppController>.Instance.Model.ShopModel.AvailableMultiplier;
			UnlockItemsForLevels(selectedLevel.Scene.Name);
		}

		private void TakeReward(LevelItem selectedLevel)
		{
			UnityEngine.Debug.Log("take reward");
			if (selectedLevel.Scene == typeof(ScorchedSparks_1_1))
			{
				DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQAg");
			}
			else if (selectedLevel.Scene == typeof(QuietLawn_2_5))
			{
				DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQAw");
			}
			else if (selectedLevel.Scene == typeof(DarkPeriphery_3_3))
			{
				DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQBA");
			}
			else if (selectedLevel.Scene == typeof(Deadweed_4_3))
			{
				DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQBQ");
			}
			else if (selectedLevel.Scene == typeof(GhoulDep_5_2))
			{
				DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQGA");
			}
			else if (selectedLevel.Scene == typeof(Train_6_3))
			{
				DonDestroyMonoSingleton<GameServiceContext>.Instance.ReportAchievment("CgkIjZj4j6kZEAIQHA");
			}
			List<ObjectiveTarget> list = UnityEngine.Object.FindObjectsOfType<ObjectiveTarget>().ToList();
			Objective model = list.Find((ObjectiveTarget obj) => obj.Model.Type == ObjectiveType.Main).Model;
			Objective model2 = list.Find((ObjectiveTarget obj) => obj.Model.Type == ObjectiveType.BonusFirst).Model;
			Objective model3 = list.Find((ObjectiveTarget obj) => obj.Model.Type == ObjectiveType.BonusSecond).Model;
			if (model == null || model2 == null || model3 == null)
			{
				UnityEngine.Debug.LogError("Whereis FUC#$$#G OBJECTIVES???");
				return;
			}
			Slugterra.Domain.App.Wallet wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			if (!model.IsComplete)
			{
				return;
			}
			LevelProperties levelProperties = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetLevelProperties(selectedLevel.Scene.Name);
			if (selectedLevel.GetObjectiveStatus(0) == 0)
			{
				levelProperties.realMainObjectiveGoldReward = levelProperties.mainObjectiveRewardGold;
				levelProperties.realMainObjectiveGemsReward = levelProperties.mainObjectiveRewardGems;
				wallet.Stars++;
			}
			else
			{
				levelProperties.realMainObjectiveGoldReward = levelProperties.mainObjectiveRepeatRewardGold;
				levelProperties.realMainObjectiveGemsReward = levelProperties.mainObjectiveRepeatRewardGems;
			}
			selectedLevel.SetObjectiveStatus(0, 1);
			if (model2.IsComplete && model3.IsComplete)
			{
				if (selectedLevel.GetObjectiveStatus(1) == 0 && selectedLevel.GetObjectiveStatus(2) == 0)
				{
					levelProperties.realAdditionalObjectiveGoldReward = levelProperties.allObjectivesCompleteAdditionalRewardGold;
					levelProperties.realAdditionalObjectiveGemsReward = levelProperties.allObjectivesCompleteAdditionalRewardGems;
				}
				else
				{
					levelProperties.realAdditionalObjectiveGoldReward = levelProperties.allObjectivesRepeatCompleteAdditionalRewardGold;
					levelProperties.realAdditionalObjectiveGemsReward = levelProperties.allObjectivesRepeatCompleteAdditionalRewardGems;
				}
			}
			if (model2.IsComplete && LevelItem.GetNewObjectiveStatus(1) == 1)
			{
				if (selectedLevel.GetObjectiveStatus(1) == 0)
				{
					levelProperties.realFirstObjectiveGoldReward = levelProperties.firstObjectiveRewardGold;
					levelProperties.realFirstObjectiveGemsReward = levelProperties.firstObjectiveRewardGems;
					wallet.Stars++;
				}
				else
				{
					levelProperties.realFirstObjectiveGoldReward = levelProperties.firstObjectiveRepeatRewardGold;
					levelProperties.realFirstObjectiveGemsReward = levelProperties.firstObjectiveRepeatRewardGems;
				}
				selectedLevel.SetObjectiveStatus(1, 1);
			}
			if (model3.IsComplete && LevelItem.GetNewObjectiveStatus(2) == 1)
			{
				if (selectedLevel.GetObjectiveStatus(2) == 0)
				{
					levelProperties.realSecondaryObjectiveGoldReward = levelProperties.secondaryObjectiveRewardGold;
					levelProperties.realSecondaryObjectiveGemsReward = levelProperties.secondaryObjectiveRewardGems;
					wallet.Stars++;
				}
				else
				{
					levelProperties.realSecondaryObjectiveGoldReward = levelProperties.secondaryObjectiveRepeatRewardGold;
					levelProperties.realSecondaryObjectiveGemsReward = levelProperties.secondaryObjectiveRepeatRewardGems;
				}
				selectedLevel.SetObjectiveStatus(2, 1);
			}
			UnityEngine.Debug.Log("GameController::TakeReward Multiplier: " + MonoSingleton<AppController>.Instance.Model.ShopModel.AvailableMultiplier);
			wallet.Gold += levelProperties.realMainObjectiveGoldReward + (levelProperties.realFirstObjectiveGoldReward + levelProperties.realSecondaryObjectiveGoldReward) * (float)MonoSingleton<AppController>.Instance.Model.ShopModel.AvailableMultiplier;
			wallet.Gems += levelProperties.realMainObjectiveGemsReward + (levelProperties.realFirstObjectiveGemsReward + levelProperties.realSecondaryObjectiveGemsReward) * (float)MonoSingleton<AppController>.Instance.Model.ShopModel.AvailableMultiplier;
			UnlockItemsForLevels(selectedLevel.Scene.Name);
		}

		private void UpdateProgress(LevelItem selectedLevel)
		{
			PlayerPrefs.SetString("lastPassedLevel", selectedLevel.Scene.Name);
			selectedLevel.Status = LevelStatus.Completed;
			MonoSingleton<AppController>.Instance.Model.ShopModel.RemoveJustUnlockedItems();
			if (selectedLevel.Type == LevelType.Action)
			{
				SetCurrentLevel(selectedLevel);
				List<LevelItem> levels = MonoSingleton<AppController>.Instance.Model.Levels;
				List<LevelItem> list = levels.FindAll((LevelItem level) => level.Order == LevelItem.Current);
				foreach (LevelItem item in list)
				{
					if (item.Status != LevelStatus.Completed)
					{
						item.Status = LevelStatus.Unlocked;
					}
				}
			}
			if (selectedLevel.Type == LevelType.Arena)
			{
				SetCurrentLevel(selectedLevel);
			}
		}

		private void SetCurrentLevel(LevelItem selectedLevel)
		{
			List<LevelItem> levels = MonoSingleton<AppController>.Instance.Model.Levels;
			LevelItem levelItem = levels.Find((LevelItem level) => level.Status == LevelStatus.Unlocked && level.Type == LevelType.Action && !level.Name.Contains("daily") && level.Order != 0);
			if (levelItem != null)
			{
				LevelItem.Current = levelItem.Order;
			}
			else if (levels.Find((LevelItem level) => level.Status == LevelStatus.Locked && level.Type == LevelType.Action && level.Order != 0) == null)
			{
				LevelItem.Current = 17;
			}
			else if (selectedLevel.Order + 1 <= 17)
			{
				LevelItem.Current = selectedLevel.Order + 1;
			}
			levelItem = levels.Find((LevelItem level) => level.Order == LevelItem.Current && level.Type == LevelType.Action && !level.Name.Contains("daily"));
			if (levelItem != null)
			{
				MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission = levelItem.Scene;
			}
		}

		private void UnlockItemsForLevels(string levelName)
		{
			List<ArmorType> armorsToUnlock = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetArmorsToUnlock(levelName);
			List<BlasterType> blastersToUnlock = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetBlastersToUnlock(levelName);
			List<SlugType> slugsToUnlock = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetSlugsToUnlock(levelName);
			List<AllyType> alliesToUnlock = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetAlliesToUnlock(levelName);
			List<string> bossesToUnlock = MonoSingleton<GameConfiguration>.Instance.LevelsConfigurationManager.GetBossesToUnlock(levelName);
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			shopModel.UnlockArmors(armorsToUnlock);
			shopModel.UnlockBlasters(blastersToUnlock);
			shopModel.UnlockSlugs(slugsToUnlock);
			shopModel.UnlockAllies(alliesToUnlock);
			shopModel.UnlockBosses(bossesToUnlock);
		}

		public void OnFinishGame(bool success = true)
		{
			if (!success && !_gameFailed)
			{
				_gameFailed = true;
				_model.GameWon = false;
				_model.GameFailed = true;
				StartCoroutine(GameFailed(2f));
			}
			else if (success)
			{
				_model.GameWon = true;
				_model.GameFailed = false;
			}
		}

		public void OnResultmultiplayerFail()
		{
			if (!result)
			{
				result = true;
				_model.GameFailed = true;
				_model.GameWon = false;
				StartCoroutine(GameFailed(0.5f));
			}
		}

		public void OnResultmultiplayerWin()
		{
			if (!result)
			{
				result = true;
				_model.GameWon = true;
				_model.GameFailed = false;
				StartCoroutine(GameWin(0.5f));
			}
		}

		public void SkipMission()
		{
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			if (currentGameLevel != null)
			{
				UpdateProgress(currentGameLevel);
				UnlockItemsForLevels(currentGameLevel.Scene.Name);
				if (currentGameLevel.Scene == typeof(ShaneTutor_0_1))
				{
					SceneLoader.LoadScene<MapSceneController>();
				}
				else
				{
					SceneLoader.LoadFromPrev<MapSceneController>(true);
				}
			}
		}

		public void IsNotMultiplayer()
		{
			_isMultiplayer = false;
		}

		protected override void OnReleaseResource()
		{
		}

		public void PlayCutScene(bool isPlaying)
		{
			_isPlayCutScene = isPlaying;
			if (isPlaying)
			{
				Model.State = GameState.CutScene;
				Model.Eli.Invulnerability = true;
			}
			else if (Model.State == GameState.CutScene)
			{
				Model.Eli.Invulnerability = false;
				Model.State = GameState.Play;
			}
			if (OnPlayCutSceneEvent__BackingField != null)
			{
				OnPlayCutSceneEvent__BackingField(isPlaying);
			}
		}

		public void SetAssistantAvatar(string Avatar)
		{
			_inGameController.AssistAvatarInfo.View.SetAvatar(Avatar);
		}

		private void OnApplicationPause(bool paused)
		{
			if (paused && !_isPlayCutScene && !_isMultiplayer && Model.State == GameState.Play && Model.State != GameState.CutScene && (ManualMonoSingleton<SlugFightUICameraView>.ManualInstance == null || !ManualMonoSingleton<SlugFightUICameraView>.ManualInstance.IsSlugFightActive))
			{
				OnPause();
			}
			if (paused && !_isPlayCutScene && _isMultiplayer && MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene == typeof(TutorialMultiplayer) && Model.State == GameState.Play && Model.State != GameState.CutScene && (ManualMonoSingleton<SlugFightUICameraView>.ManualInstance == null || !ManualMonoSingleton<SlugFightUICameraView>.ManualInstance.IsSlugFightActive))
			{
				OnPause();
			}
		}

		private void OnBack()
		{
			if (Model == null)
			{
				return;
			}
			if (Model.State == GameState.Play && !_isMultiplayer)
			{
				if ((!(SceneController != null) || MonoSingleton<AppController>.Instance.Model.GetLevelItem(SceneController.GetType()).Type != LevelType.Arena || !(MonoSingleton<DuelManager>.Instance.StateMachine.CurrentState is SlugFightState)) && MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene != typeof(ShaneTutor_0_1) && MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene != typeof(ScorchedSparksTutorial_1_1) && MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene != typeof(TutorialMultiplayer))
				{
					OnPause();
				}
			}
			else if (!_isMultiplayer && Model.State == GameState.Pause)
			{
				OnPlay();
			}
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (Model.State == GameState.Play && !Model.Eli.IsDead())
			{
				MonoSingleton<MissionStatisticsController>.Instance.Model.Time += Time.deltaTime;
			}
		}
	}
}

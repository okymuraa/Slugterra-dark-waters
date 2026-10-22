using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.Scene;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.Game.UI;
using Slugterra.Domain.UI.Game;
using Slugterra.Scene;
using Slugterra.Scene.Game;
using Slugterra.Scene.Multiplayer;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class InGameView : UIView
	{
		[Serializable]
		private struct ChangePlatformObjects
		{
			public GameObject _objectTVOS;

			public GameObject _objectDefault;

			public ChangePlatformObjects(GameObject objectTVOS, GameObject objectDefault)
			{
				_objectTVOS = objectTVOS;
				_objectDefault = objectDefault;
			}

			public void SetAcriveObjects()
			{
				bool flag = false;
				flag = false;
				if (_objectTVOS != null)
				{
					_objectTVOS.SetActive(flag);
				}
				if (_objectDefault != null)
				{
					_objectDefault.SetActive(!flag);
				}
			}
		}

		private static readonly float BoostBarArenaPositionX = 0.045f;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPauseEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnMinimapEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnReleaseEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnSkipMissionEvent__BackingField;

		[SerializeField]
		private ObjectiveCompleteMessageView _objectiveCompleteMessage;

		[SerializeField]
		private CountdownWarningMessageView _countdownWarningMessage;

		[SerializeField]
		private GameObject _radarObject;

		[SerializeField]
		private GameObject _releaseBar;

		[SerializeField]
		private GameObject _suvivalInfoPanel;

		[SerializeField]
		private GameObject _suvivalInfoPanelTVOS;

		[SerializeField]
		private GameObject _leftArrowObject;

		[SerializeField]
		private GameObject _rightArrowObject;

		[SerializeField]
		private GameObject _pauseButtonCenter;

		[SerializeField]
		private GameObject _pauseButtonSkipTutorial;

		[SerializeField]
		private GameObject _pauseButtonEndMatch;

		[SerializeField]
		private GameObject _inventoryBarControllerArena;

		[SerializeField]
		private GameObject _inventoryBarControllerAction;

		[SerializeField]
		private DamageController _damageController;

		private GameController _gameController;

		[SerializeField]
		private ChangePlatformObjects _btns;

		private LevelItem levelItem;

		private PopupConfirmView _popup;

		public ObjectiveCompleteMessageView ObjectiveCompleteMessage
		{
			get
			{
				return _objectiveCompleteMessage;
			}
		}

		public CountdownWarningMessageView CountdownWarningMessage
		{
			get
			{
				return _countdownWarningMessage;
			}
		}

		public GameObject ReleaseBar
		{
			get
			{
				return _releaseBar;
			}
			set
			{
				_releaseBar = value;
			}
		}

		public GameObject InventoryBarControllerArena
		{
			get
			{
				return _inventoryBarControllerArena;
			}
		}

		public GameObject InventoryBarControllerAction
		{
			get
			{
				return _inventoryBarControllerAction;
			}
		}

		public DamageController DamageController
		{
			get
			{
				return _damageController;
			}
		}

		public event Action OnPauseEvent
		{
			add
			{
				Action action = OnPauseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPauseEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnPauseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPauseEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnMinimapEvent
		{
			add
			{
				Action action = OnMinimapEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMinimapEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnMinimapEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMinimapEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnReleaseEvent
		{
			add
			{
				Action action = OnReleaseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnReleaseEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnReleaseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnReleaseEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnSkipMissionEvent
		{
			add
			{
				Action action = OnSkipMissionEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSkipMissionEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnSkipMissionEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSkipMissionEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private void OnEnable()
		{
			_btns.SetAcriveObjects();
			levelItem = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			Attribute[] customAttributes = Attribute.GetCustomAttributes(levelItem.Scene);
			LeaderBoardIDAttribute leaderBoardIDAttribute = null;
			_popup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
			Attribute[] array = customAttributes;
			foreach (Attribute attribute in array)
			{
				if (attribute is LeaderBoardIDAttribute)
				{
					leaderBoardIDAttribute = attribute as LeaderBoardIDAttribute;
				}
			}
			if (leaderBoardIDAttribute != null && !string.IsNullOrEmpty(leaderBoardIDAttribute.LeaderBoardId))
			{
				_suvivalInfoPanel.SetActive(true);
			}
			DonDestroyMonoSingleton<InputManager>.Instance.OpenMiniMapEvent += OnBtnRadarClick;
		}

		private void OnDisable()
		{
			if (DonDestroyMonoSingleton<InputManager>.IsAlive)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.OpenMiniMapEvent -= OnBtnRadarClick;
			}
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			InGameModel inGameModel = model as InGameModel;
			if (inGameModel == null)
			{
				return;
			}
			if (_gameController == null)
			{
				_gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			}
			if (inGameModel.LevelType == LevelType.Arena || _gameController.IsMultiplayer)
			{
				if (_radarObject != null)
				{
					_radarObject.SetActive(false);
				}
				if (!_gameController.IsMultiplayer)
				{
					_leftArrowObject.SetActive(inGameModel.IsLeftArrow);
					_rightArrowObject.SetActive(inGameModel.IsRightArrow);
				}
			}
			if (MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene == typeof(ShaneTutor_0_1) || MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene == typeof(ScorchedSparksTutorial_1_1) || MonoSingleton<AppController>.Instance.GetCurrentGameLevel().Scene == typeof(TutorialMultiplayer))
			{
				_pauseButtonSkipTutorial.SetActive(true);
				_pauseButtonCenter.SetActive(false);
				_pauseButtonEndMatch.SetActive(false);
			}
			else
			{
				_pauseButtonSkipTutorial.SetActive(false);
				_pauseButtonCenter.SetActive(!_gameController.IsMultiplayer);
				_pauseButtonEndMatch.SetActive(_gameController.IsMultiplayer);
			}
		}

		private void OnBtnPauseClick()
		{
			if (_gameController.IsMultiplayer)
			{
				OnLeaveMultiplayer();
			}
			else if (OnPauseEvent__BackingField != null)
			{
				OnPauseEvent__BackingField();
			}
		}

		private void OnBtnSkipMissionClick()
		{
			if (OnSkipMissionEvent__BackingField != null)
			{
				OnSkipMissionEvent__BackingField();
			}
		}

		public void OnBtnRadarClick()
		{
			if (OnMinimapEvent__BackingField != null && GetLeaderBoard() == null)
			{
				SlugterraSceneController slugterraSceneController = UnityEngine.Object.FindObjectOfType<SlugterraSceneController>();
				LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
				if (currentGameLevel.Type != LevelType.Arena)
				{
					OnMinimapEvent__BackingField();
				}
			}
		}

		private string GetLeaderBoard()
		{
			SlugterraSceneController slugterraSceneController = UnityEngine.Object.FindObjectOfType<SlugterraSceneController>();
			LeaderBoardIDAttribute leaderBoardIDAttribute = Attribute.GetCustomAttribute(slugterraSceneController.GetType(), typeof(LeaderBoardIDAttribute)) as LeaderBoardIDAttribute;
			if (leaderBoardIDAttribute != null && !string.IsNullOrEmpty(leaderBoardIDAttribute.LeaderBoardId))
			{
				return leaderBoardIDAttribute.LeaderBoardId;
			}
			return null;
		}

		private void OnBtnReleaseClick()
		{
			if (OnReleaseEvent__BackingField != null)
			{
				OnReleaseEvent__BackingField();
			}
		}

		private void OnBtnEndMpGameClick()
		{
			PhotonNetwork.Disconnect();
			SceneLoader.LoadScene<MainSceneController>(true);
		}

		private void OnLeaveMultiplayer()
		{
			if (_popup == null)
			{
				_popup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
				if (_popup == null)
				{
					PhotonNetwork.Disconnect();
					if (ConnectAndJoinRandom.BattleBegun)
					{
						_gameController.OnResultmultiplayerFail();
					}
					else
					{
						SceneLoader.LoadFromPrev<MultiplayerSceneController>(true);
					}
					return;
				}
			}
			_popup.Show("alert_leave_multiplayer", () =>
			{
				PhotonNetwork.Disconnect();
				if (ConnectAndJoinRandom.BattleBegun)
				{
					_gameController.OnResultmultiplayerFail();
				}
				else
				{
					SceneLoader.LoadFromPrev<MultiplayerSceneController>(true);
				}
			});
		}
	}
}

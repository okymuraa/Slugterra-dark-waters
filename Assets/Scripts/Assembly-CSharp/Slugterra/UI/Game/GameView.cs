using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain;
using Slugterra.Domain.Game.UI;
using Slugterra.Scene.Multiplayer;
using Slugterra.UI.Multiplayer;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class GameView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPauseEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPlayEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnRespawnEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnMinimapEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnSkipMissionEvent__BackingField;

		private InGameView _inGameView;

		private PauseView _pauseView;

		private LoseView _loseView;

		private WinView _winView;

		private SurvivalView _survivalView;

		private MinimapView _minimapView;

		private ResultMultiplayerLoseView _multiplayerLoseView;

		private ResultMultiplayerWinView _multiplayerWinView;

		private ResultMultiplayerDrawView _multiplayerDrawView;

		private UIView _currentView;

		private bool _disableOnce;

		private UnityEngine.Camera _mainCamera;

		public ResultMultiplayerLoseView MultiplayerLoseView
		{
			get
			{
				return _multiplayerLoseView;
			}
		}

		public ResultMultiplayerWinView MultiplayerWinView
		{
			get
			{
				return _multiplayerWinView;
			}
		}

		public ResultMultiplayerDrawView MultiplayerDrawView
		{
			get
			{
				return _multiplayerDrawView;
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

		public event Action OnPlayEvent
		{
			add
			{
				Action action = OnPlayEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnPlayEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnRespawnEvent
		{
			add
			{
				Action action = OnRespawnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRespawnEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnRespawnEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRespawnEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
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

		private void Awake()
		{
			_inGameView = UnityEngine.Object.FindObjectOfType<InGameView>();
			_currentView = _inGameView;
			_pauseView = UnityEngine.Object.FindObjectOfType<PauseView>();
			_loseView = UnityEngine.Object.FindObjectOfType<LoseView>();
			_winView = UnityEngine.Object.FindObjectOfType<WinView>();
			_minimapView = UnityEngine.Object.FindObjectOfType<MinimapView>();
			_survivalView = UnityEngine.Object.FindObjectOfType<SurvivalView>();
			_multiplayerLoseView = UnityEngine.Object.FindObjectOfType<ResultMultiplayerLoseView>();
			_multiplayerWinView = UnityEngine.Object.FindObjectOfType<ResultMultiplayerWinView>();
			_multiplayerDrawView = UnityEngine.Object.FindObjectOfType<ResultMultiplayerDrawView>();
			_mainCamera = UnityEngine.Camera.main;
		}

		protected override void OnStart()
		{
			DisableViews();
		}

		private void DisableViews()
		{
			if (!_disableOnce)
			{
				if (_pauseView != null)
				{
					_pauseView.gameObject.SetActive(false);
				}
				_loseView.gameObject.SetActive(false);
				_winView.gameObject.SetActive(false);
				if (_multiplayerLoseView != null)
				{
					_multiplayerLoseView.gameObject.SetActive(false);
				}
				if (_multiplayerWinView != null)
				{
					_multiplayerWinView.gameObject.SetActive(false);
				}
				if (_multiplayerDrawView != null)
				{
					_multiplayerDrawView.gameObject.SetActive(false);
				}
				if (_survivalView != null)
				{
					_survivalView.gameObject.SetActive(false);
				}
				if (_minimapView != null)
				{
					_minimapView.gameObject.SetActive(false);
				}
				_disableOnce = true;
			}
		}

		private void OnEnable()
		{
			if (_pauseView != null)
			{
				_pauseView.OnPlayEvent += OnPlay;
			}
			_inGameView.OnPauseEvent += OnPause;
			_inGameView.OnMinimapEvent += OnMinimap;
			_loseView.OnRespawnEvent += OnRespawn;
			_inGameView.OnSkipMissionEvent += OnSkipMission;
			DonDestroyMonoSingleton<InputManager>.Instance.OnSkipMissionEvent += OnSkipMission;
			if (_minimapView != null)
			{
				_minimapView.OnBackEvent += OnPlay;
			}
		}

		private void OnDisable()
		{
			if (_pauseView != null)
			{
				_pauseView.OnPlayEvent -= OnPlay;
			}
			_inGameView.OnPauseEvent -= OnPause;
			_inGameView.OnMinimapEvent -= OnMinimap;
			_loseView.OnRespawnEvent -= OnRespawn;
			_inGameView.OnSkipMissionEvent -= OnSkipMission;
			DonDestroyMonoSingleton<InputManager>.Instance.OnSkipMissionEvent -= OnSkipMission;
			if (_minimapView != null)
			{
				_minimapView.OnBackEvent -= OnPlay;
			}
		}

		private void OnPlay()
		{
			if (OnPlayEvent__BackingField != null)
			{
				OnPlayEvent__BackingField();
			}
		}

		private void OnPause()
		{
			if (OnPauseEvent__BackingField != null)
			{
				OnPauseEvent__BackingField();
			}
		}

		private void OnSkipMission()
		{
			if (OnSkipMissionEvent__BackingField != null)
			{
				OnSkipMissionEvent__BackingField();
			}
		}

		private void OnRespawn()
		{
			if (OnRespawnEvent__BackingField != null)
			{
				OnRespawnEvent__BackingField();
			}
		}

		private void OnMinimap()
		{
			if (OnMinimapEvent__BackingField != null)
			{
				OnMinimapEvent__BackingField();
			}
		}

		protected override void OnApplyModel(object model)
		{
			GameModel gameModel = model as GameModel;
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			bool flag = currentGameLevel.Scene == typeof(DesertBase1Multiplayer) || currentGameLevel.Scene == typeof(EastSet1Multiplayer);
			if (gameModel.State == GameState.Play)
			{
				ActivateView(_inGameView);
			}
			else if (gameModel.State == GameState.Pause)
			{
				ActivateView(_pauseView);
			}
			else if (gameModel.State == GameState.Complete)
			{
				if (flag)
				{
					ActivateView(_multiplayerWinView);
				}
				else
				{
					ActivateView(_winView);
				}
			}
			else if (gameModel.State == GameState.Defeat)
			{
				if (flag)
				{
					ActivateView(_multiplayerLoseView);
				}
				else
				{
					ActivateView(_loseView);
				}
			}
			else if (gameModel.State == GameState.Minimap)
			{
				ActivateView(_minimapView);
			}
			else if (gameModel.State == GameState.Survival)
			{
				ActivateView(_survivalView);
			}
		}

		private void ActivateView(UIView view)
		{
			if (_currentView == null)
			{
				return;
			}
			if (_mainCamera != null)
			{
				if (view == _minimapView)
				{
					_mainCamera.enabled = false;
				}
				if (_currentView == _minimapView)
				{
					_mainCamera.enabled = true;
				}
			}
			if (_currentView != view)
			{
				_currentView.gameObject.SetActive(false);
				_currentView = view;
			}
			if (!_currentView.gameObject.activeSelf)
			{
				_currentView.gameObject.SetActive(true);
			}
		}
	}
}

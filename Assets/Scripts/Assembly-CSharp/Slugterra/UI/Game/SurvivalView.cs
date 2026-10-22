using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Plugins;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class SurvivalView : UIView
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

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnSharePhotoEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnShareVideoEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnMainMenuEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnRestartEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnHelpEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnNextEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBtnGameServiceEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBtnChallengeEvent__BackingField;

		[SerializeField]
		private GameObject _shareVideoBtns;

		[SerializeField]
		private GameObject _sharePhotoBtn;

		private GameController _gameController;

		private GameSetting _settings;

		private bool doubleCheckForReplayKit;

		[SerializeField]
		private ChangePlatformObjects _btns;

		public GameObject ShareVideo
		{
			get
			{
				return _shareVideoBtns;
			}
		}

		public GameObject SharePhoto
		{
			get
			{
				return _sharePhotoBtn;
			}
		}

		public event Action OnSharePhotoEvent
		{
			add
			{
				Action action = OnSharePhotoEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSharePhotoEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnSharePhotoEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSharePhotoEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnShareVideoEvent
		{
			add
			{
				Action action = OnShareVideoEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnShareVideoEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnShareVideoEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnShareVideoEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnMainMenuEvent
		{
			add
			{
				Action action = OnMainMenuEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMainMenuEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnMainMenuEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMainMenuEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnRestartEvent
		{
			add
			{
				Action action = OnRestartEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRestartEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnRestartEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnRestartEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnHelpEvent
		{
			add
			{
				Action action = OnHelpEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnHelpEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnHelpEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnHelpEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnNextEvent
		{
			add
			{
				Action action = OnNextEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNextEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnNextEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnNextEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnBtnGameServiceEvent
		{
			add
			{
				Action action = OnBtnGameServiceEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnGameServiceEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBtnGameServiceEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnGameServiceEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnBtnChallengeEvent
		{
			add
			{
				Action action = OnBtnChallengeEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnChallengeEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBtnChallengeEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnChallengeEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnStart()
		{
			_settings = MonoSingleton<AppController>.Instance.Model.GameSetting;
			_btns.SetAcriveObjects();
		}

		private void OnEnable()
		{
			if (_gameController == null)
			{
				_gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			}
			if (_settings == null)
			{
				_settings = MonoSingleton<AppController>.Instance.Model.GameSetting;
			}
			_shareVideoBtns.SetActive(false);
			_sharePhotoBtn.SetActive(true);
			if (_gameController != null && _gameController.Model.State == GameState.Survival)
			{
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.LevelComplete();
			}
		}

		private void OnBtnSharePhotoClick()
		{
			if (OnSharePhotoEvent__BackingField != null)
			{
				OnSharePhotoEvent__BackingField();
			}
		}

		private void OnBtnShareVideoClick()
		{
			if (OnShareVideoEvent__BackingField != null)
			{
				OnShareVideoEvent__BackingField();
			}
		}

		private void OnBtnMainMenuClick()
		{
			LevelItem currentGameLevel = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			if (currentGameLevel != null && currentGameLevel.Status == LevelStatus.Completed)
			{
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.ExitFromLevel();
			}
			else
			{
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.LevelUncomplete();
			}
			if (OnMainMenuEvent__BackingField != null)
			{
				OnMainMenuEvent__BackingField();
			}
		}

		private void OnBtnRestartClick()
		{
			DonDestroyMonoSingleton<AnalyticsManager>.Instance.RestartLevel();
			if (OnRestartEvent__BackingField != null)
			{
				OnRestartEvent__BackingField();
			}
		}

		private void OnBtnHelpClick()
		{
			if (OnHelpEvent__BackingField != null)
			{
				OnHelpEvent__BackingField();
			}
		}

		private void OnBtnNextClick()
		{
			if (OnNextEvent__BackingField != null)
			{
				OnNextEvent__BackingField();
			}
		}

		private void OnBtnGameServiceClick()
		{
			if (OnBtnGameServiceEvent__BackingField != null)
			{
				OnBtnGameServiceEvent__BackingField();
			}
		}

		private void OnBtnChallengeClick()
		{
			if (OnBtnChallengeEvent__BackingField != null)
			{
				OnBtnChallengeEvent__BackingField();
			}
		}
	}
}

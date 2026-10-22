using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Plugins;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class PauseView : UIView
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
		private Action OnPlayEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnMainMenuEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnMapEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnRestartEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnSurvivalEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBtnGameServiceEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnHelpEvent__BackingField;

		[SerializeField]
		private GameObject _saleInStore;

		[SerializeField]
		private GameObject _survivalGoals;

		[SerializeField]
		private GameObject _compaingGoals;

		[SerializeField]
		private GameObject _survivalMenu;

		[SerializeField]
		private GameObject _compaingMenu;

		[SerializeField]
		private GameObject _survivalMenuTVOS;

		[SerializeField]
		private GameObject _compaingMenuTVOS;

		private GameSetting _settings;

		[SerializeField]
		private ChangePlatformObjects _btns;

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

		public event Action OnMapEvent
		{
			add
			{
				Action action = OnMapEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMapEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnMapEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnMapEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
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

		public event Action OnSurvivalEvent
		{
			add
			{
				Action action = OnSurvivalEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSurvivalEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnSurvivalEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnSurvivalEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
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

		protected override void OnStart()
		{
			_settings = MonoSingleton<AppController>.Instance.Model.GameSetting;
			_btns.SetAcriveObjects();
		}

		private void OnEnable()
		{
			if (_settings == null)
			{
				_settings = MonoSingleton<AppController>.Instance.Model.GameSetting;
			}
			string leaderBoardID = SurvivalController.GetLeaderBoardID();
			if ((bool)_survivalGoals && ((bool)_compaingGoals & (bool)_survivalMenu & (bool)_compaingMenu))
			{
				if (SurvivalController.IsSurvival(MonoSingleton<AppController>.Instance.Model.MissionSetting.Mission))
				{
					_survivalMenu.SetActive(true);
					_compaingMenu.SetActive(false);
					_survivalGoals.SetActive(true);
					_compaingGoals.SetActive(false);
				}
				else
				{
					_survivalMenu.SetActive(false);
					_compaingMenu.SetActive(true);
					_survivalGoals.SetActive(false);
					_compaingGoals.SetActive(true);
				}
			}
			StartCoroutine(UpdateViews());
		}

		private IEnumerator UpdateViews()
		{
			yield return StartCoroutine(CollectionUtils.WaitForRealSeconds(0.5f));
		}

		private void OnBtnMapClick()
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
			LevelItem currentGameLevel2 = MonoSingleton<AppController>.Instance.GetCurrentGameLevel();
			if (currentGameLevel2.Name == "survival")
			{
				if (OnSurvivalEvent__BackingField != null)
				{
					OnSurvivalEvent__BackingField();
				}
			}
			else if (OnMapEvent__BackingField != null)
			{
				OnMapEvent__BackingField();
			}
		}

		private void OnBtnMenuClick()
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

		private void OnBtnResumeClick()
		{
			if (OnPlayEvent__BackingField != null)
			{
				OnPlayEvent__BackingField();
			}
		}

		private void OnBtnHelpClick()
		{
			if (OnHelpEvent__BackingField != null)
			{
				OnHelpEvent__BackingField();
			}
		}

		private void OnBtnGameServiceClick()
		{
			if (OnBtnGameServiceEvent__BackingField != null)
			{
				OnBtnGameServiceEvent__BackingField();
			}
		}
	}
}

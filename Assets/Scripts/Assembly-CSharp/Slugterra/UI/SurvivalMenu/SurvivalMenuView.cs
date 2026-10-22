using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using Slugterra.UI.Survival;
using UnityEngine;

namespace Slugterra.UI.SurvivalMenu
{
	public class SurvivalMenuView : UIView, IObserver
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBackEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBtnGameServiceEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBtnNextClickEvent__BackingField;

		[SerializeField]
		private GameObject _mapItemPrefab;

		[SerializeField]
		private tk2dUIScrollableArea _scrollArea;

		[SerializeField]
		private BadgeIconView _challengeCounter;

		[SerializeField]
		private GameObject _saleInStore;

		[SerializeField]
		private GameObject _NextButtonOnState;

		[SerializeField]
		private GameObject _NextButtonOffState;

		public GameObject Tutorial;

		private List<SurvivalMapItemView> _rivalItemUI = new List<SurvivalMapItemView>();

		public event Action OnBackEvent
		{
			add
			{
				Action action = OnBackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBackEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBackEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
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

		public event Action OnBtnNextClickEvent
		{
			add
			{
				Action action = OnBtnNextClickEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnNextClickEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnBtnNextClickEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBtnNextClickEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private void OnEnable()
		{
			MonoSingleton<AppController>.Instance.Model.MissionSetting.AddObserver(this);
		}

		private void OnDisable()
		{
			if (MonoSingleton<AppController>.IsAlive)
			{
				MonoSingleton<AppController>.Instance.Model.MissionSetting.RemoveObserver(this);
			}
		}

		private void OnBtnBackClick()
		{
			if (OnBackEvent__BackingField != null)
			{
				OnBackEvent__BackingField();
			}
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			_challengeCounter.gameObject.SetActive(false);
			if (!(model is List<LevelItem>))
			{
				return;
			}
			List<LevelItem> list = model as List<LevelItem>;
			if (_scrollArea.contentContainer.transform.childCount == 0)
			{
				int num = 0;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].Name == "survival")
					{
						GameObject gameObject = UnityEngine.Object.Instantiate(_mapItemPrefab);
						gameObject.transform.parent = _scrollArea.contentContainer.transform;
						gameObject.transform.localPosition = new Vector3((float)num * 2.88f, 0f, 0f);
						SurvivalMapItemView component = gameObject.GetComponent<SurvivalMapItemView>();
						component.ApplyModel(list[i]);
						_rivalItemUI.Add(component);
						num++;
					}
				}
				_scrollArea.ContentLength = (float)num * 2.88f;
				if (Tutorial != null && Tutorial.activeSelf)
				{
					Tutorial.SendMessage("StartScene", _rivalItemUI);
				}
			}
			if (DonDestroyMonoSingleton<GameServiceContext>.Instance.GameServiceChallenges != null && DonDestroyMonoSingleton<GameServiceContext>.Instance.GameServiceChallenges.Count <= 0)
			{
			}
		}

		private void OnBtnGameServiceClick()
		{
			if (OnBtnGameServiceEvent__BackingField != null)
			{
				OnBtnGameServiceEvent__BackingField();
			}
		}

		private void OnBtnNextClick()
		{
			if (OnBtnNextClickEvent__BackingField != null)
			{
				OnBtnNextClickEvent__BackingField();
			}
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is MissionSetting)
			{
				MissionSetting missionSetting = observable as MissionSetting;
				if (missionSetting.Mission != null)
				{
					_NextButtonOnState.SetActive(true);
					_NextButtonOffState.SetActive(false);
				}
			}
		}
	}
}

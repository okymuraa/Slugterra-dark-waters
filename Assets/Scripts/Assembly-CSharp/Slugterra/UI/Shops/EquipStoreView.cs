using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.UI;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class EquipStoreView : UIView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnBackEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnGiftsEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPlayEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnPlayFailEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnGameServiceEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Func<Type, bool> OnTabEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Func<Type, SlugGroup, bool> OnTabSelectElementEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Func<Type, bool> OnAssistantTabSelectElementEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Func<Type, bool> OnBlasterTabSelectElementEvent__BackingField;

		[SerializeField]
		private SwapGameObjectsState _swapGameObjectsState;

		[SerializeField]
		private FreeBonusButton _freeBonusBtn;

		[SerializeField]
		private GameObject _panelBackAndroid;

		[SerializeField]
		private GameObject _panelBackOther;

		[SerializeField]
		private GameObject _saleInStore;

		public SwapGameObjectsState SwapGameObjectsState
		{
			get
			{
				return _swapGameObjectsState;
			}
		}

		public FreeBonusButton FreeBonustBtn
		{
			get
			{
				return _freeBonusBtn;
			}
		}

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

		public event Action OnGiftsEvent
		{
			add
			{
				Action action = OnGiftsEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnGiftsEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnGiftsEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnGiftsEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
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

		public event Action OnPlayFailEvent
		{
			add
			{
				Action action = OnPlayFailEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayFailEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnPlayFailEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnPlayFailEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action OnGameServiceEvent
		{
			add
			{
				Action action = OnGameServiceEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnGameServiceEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnGameServiceEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnGameServiceEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Func<Type, bool> OnTabEvent
		{
			add
			{
				Func<Type, bool> func = OnTabEvent__BackingField;
				Func<Type, bool> func2;
				do
				{
					func2 = func;
					func = Interlocked.CompareExchange(ref OnTabEvent__BackingField, (Func<Type, bool>)Delegate.Combine(func2, value), func);
				}
				while ((object)func != func2);
			}
			remove
			{
				Func<Type, bool> func = OnTabEvent__BackingField;
				Func<Type, bool> func2;
				do
				{
					func2 = func;
					func = Interlocked.CompareExchange(ref OnTabEvent__BackingField, (Func<Type, bool>)Delegate.Remove(func2, value), func);
				}
				while ((object)func != func2);
			}
		}

		public event Func<Type, SlugGroup, bool> OnTabSelectElementEvent
		{
			add
			{
				Func<Type, SlugGroup, bool> func = OnTabSelectElementEvent__BackingField;
				Func<Type, SlugGroup, bool> func2;
				do
				{
					func2 = func;
					func = Interlocked.CompareExchange(ref OnTabSelectElementEvent__BackingField, (Func<Type, SlugGroup, bool>)Delegate.Combine(func2, value), func);
				}
				while ((object)func != func2);
			}
			remove
			{
				Func<Type, SlugGroup, bool> func = OnTabSelectElementEvent__BackingField;
				Func<Type, SlugGroup, bool> func2;
				do
				{
					func2 = func;
					func = Interlocked.CompareExchange(ref OnTabSelectElementEvent__BackingField, (Func<Type, SlugGroup, bool>)Delegate.Remove(func2, value), func);
				}
				while ((object)func != func2);
			}
		}

		public event Func<Type, bool> OnAssistantTabSelectElementEvent
		{
			add
			{
				Func<Type, bool> func = OnAssistantTabSelectElementEvent__BackingField;
				Func<Type, bool> func2;
				do
				{
					func2 = func;
					func = Interlocked.CompareExchange(ref OnAssistantTabSelectElementEvent__BackingField, (Func<Type, bool>)Delegate.Combine(func2, value), func);
				}
				while ((object)func != func2);
			}
			remove
			{
				Func<Type, bool> func = OnAssistantTabSelectElementEvent__BackingField;
				Func<Type, bool> func2;
				do
				{
					func2 = func;
					func = Interlocked.CompareExchange(ref OnAssistantTabSelectElementEvent__BackingField, (Func<Type, bool>)Delegate.Remove(func2, value), func);
				}
				while ((object)func != func2);
			}
		}

		public event Func<Type, bool> OnBlasterTabSelectElementEvent
		{
			add
			{
				Func<Type, bool> func = OnBlasterTabSelectElementEvent__BackingField;
				Func<Type, bool> func2;
				do
				{
					func2 = func;
					func = Interlocked.CompareExchange(ref OnBlasterTabSelectElementEvent__BackingField, (Func<Type, bool>)Delegate.Combine(func2, value), func);
				}
				while ((object)func != func2);
			}
			remove
			{
				Func<Type, bool> func = OnBlasterTabSelectElementEvent__BackingField;
				Func<Type, bool> func2;
				do
				{
					func2 = func;
					func = Interlocked.CompareExchange(ref OnBlasterTabSelectElementEvent__BackingField, (Func<Type, bool>)Delegate.Remove(func2, value), func);
				}
				while ((object)func != func2);
			}
		}

		protected override void OnStart()
		{
			base.OnStart();
			_panelBackAndroid.SetActive(false);
			_panelBackOther.SetActive(true);
		}

		private void OnBtnBackClick()
		{
			if (OnBackEvent__BackingField != null)
			{
				OnBackEvent__BackingField();
			}
		}

		private void OnBtnGiftsClick()
		{
			if (OnGiftsEvent__BackingField != null)
			{
				OnGiftsEvent__BackingField();
			}
		}

		private void OnBtnGameServiceClick()
		{
			if (OnGameServiceEvent__BackingField != null)
			{
				OnGameServiceEvent__BackingField();
			}
		}

		private void OnBtnPreviewClick(tk2dUIToggleButton toggle)
		{
			if (OnTabEvent__BackingField != null && !OnTabEvent__BackingField(typeof(EliPreviewTabController)))
			{
				toggle.IsOn = !toggle.IsOn;
			}
		}

		private void OnBtnWeaponClick(tk2dUIToggleButton toggle)
		{
			if (OnTabEvent__BackingField != null && !OnTabEvent__BackingField(typeof(BlastersTabController)))
			{
				toggle.IsOn = !toggle.IsOn;
			}
		}

		private void OnBtnWeaponClickExternal()
		{
			if (OnTabEvent__BackingField != null)
			{
				OnBlasterTabSelectElementEvent__BackingField(typeof(BlastersTabController));
			}
		}

		private void OnBtnSlugsAtackClick()
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabSelectElementEvent__BackingField(typeof(SlugsTabController), SlugGroup.Attack);
			}
		}

		private void OnBtnSlugsDefenseClick()
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabSelectElementEvent__BackingField(typeof(SlugsTabController), SlugGroup.Control);
			}
		}

		private void OnBtnSlugsSpecialClick()
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabSelectElementEvent__BackingField(typeof(SlugsTabController), SlugGroup.Special);
			}
		}

		private void OnBtnSlugsClick(tk2dUIToggleButton toggle)
		{
			if (OnTabEvent__BackingField != null && !OnTabEvent__BackingField(typeof(SlugsTabController)))
			{
				toggle.IsOn = !toggle.IsOn;
			}
		}

		private void OnBtnArmorClick(tk2dUIToggleButton toggle)
		{
			if (OnTabEvent__BackingField != null && !OnTabEvent__BackingField(typeof(ArmorsTabController)))
			{
				toggle.IsOn = !toggle.IsOn;
			}
		}

		private void OnBtnAssistantsClickEli()
		{
			if (OnTabEvent__BackingField != null)
			{
				MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
				if (MonoSingleton<AppController>.Instance.Model.GetLevelItem(missionSetting.Mission).Type == LevelType.Arena)
				{
					OnAssistantTabSelectElementEvent__BackingField(typeof(FoesTabController));
				}
				else
				{
					OnAssistantTabSelectElementEvent__BackingField(typeof(AssistantsTabController));
				}
			}
		}

		private void OnBtnAssistantsClick(tk2dUIToggleButton toggle)
		{
			if (OnTabEvent__BackingField != null && !OnTabEvent__BackingField(typeof(AssistantsTabController)))
			{
				toggle.IsOn = !toggle.IsOn;
			}
		}

		private void OnBtnBoostsClick(tk2dUIToggleButton toggle)
		{
			if (OnTabEvent__BackingField != null && !OnTabEvent__BackingField(typeof(BoostsTabController)))
			{
				toggle.IsOn = !toggle.IsOn;
			}
		}

		private void OnBtnRecommendationsClick(tk2dUIToggleButton toggle)
		{
			if (OnTabEvent__BackingField != null && !OnTabEvent__BackingField(typeof(RecommendationsTabController)))
			{
				toggle.IsOn = !toggle.IsOn;
			}
		}

		private void OnBtnBoostsClickTutorial()
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabEvent__BackingField(typeof(BoostsTabController));
			}
		}

		private void OnBtnFoesClick(tk2dUIToggleButton toggle)
		{
			if (OnTabEvent__BackingField != null && !OnTabEvent__BackingField(typeof(FoesTabController)))
			{
				toggle.IsOn = !toggle.IsOn;
			}
		}

		private void OnBtnRecommendedClick()
		{
		}

		private void OnBtnStartClick()
		{
			if (OnPlayEvent__BackingField != null)
			{
				OnPlayEvent__BackingField();
			}
		}

		private void OnBtnFailClick()
		{
			if (OnPlayFailEvent__BackingField != null)
			{
				OnPlayFailEvent__BackingField();
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Core.Input;
using AppsMinistry.InAppPurchases;
using AppsMinistry.UI;
using Slugterra.App;
using Slugterra.Domain;
using Slugterra.UI.Game;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class ShopIAPView : PopupView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action OnCloseEvent__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<InappTabEnum> OnTabEvent__BackingField;

		[SerializeField]
		private List<tk2dUIToggleButton> tabButtons;

		[SerializeField]
		private tk2dUIScrollableArea[] _scrollAreas;

		private tk2dUIScrollableArea _selectedScrollArea;

		[SerializeField]
		private GameObject _iapItemPrefab;

		[SerializeField]
		private GameObject _iapVideoItemPrefab;

		[SerializeField]
		private GameObject _subscriptionItemPrefab;

		[SerializeField]
		private GameObject _gemsItemPrefab;

		[SerializeField]
		private GameObject _gemsVideoItemPrefab;

		[SerializeField]
		private GameObject _ticketsItemPrefab;

		[SerializeField]
		private GameObject _adsPanel;

		[SerializeField]
		private TextMesh _rewardText;

		private GameController _gameController;

		public TextMesh RewardText
		{
			get
			{
				return _rewardText;
			}
		}

		public event Action OnCloseEvent
		{
			add
			{
				Action action = OnCloseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCloseEvent__BackingField, (Action)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action action = OnCloseEvent__BackingField;
				Action action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnCloseEvent__BackingField, (Action)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public event Action<InappTabEnum> OnTabEvent
		{
			add
			{
				Action<InappTabEnum> action = OnTabEvent__BackingField;
				Action<InappTabEnum> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTabEvent__BackingField, (Action<InappTabEnum>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<InappTabEnum> action = OnTabEvent__BackingField;
				Action<InappTabEnum> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnTabEvent__BackingField, (Action<InappTabEnum>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		protected override void OnCreate()
		{
			MonoSingleton<Localization>.Instance.ConfigTextMesh(_rewardText);
			MonoSingleton<AppController>.Instance.Model.ShopModel.OnNotEnoughGoldEvent += OnNotEnoughGold;
			MonoSingleton<AppController>.Instance.Model.ShopModel.OnNotEnoughGemsEvent += OnNotEnoughGems;
		}

		protected override void OnStart()
		{
			if (_gameController == null)
			{
				_gameController = UnityEngine.Object.FindObjectOfType<GameController>();
			}
			gameObject.SetActive(false);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (_selectedScrollArea != null)
			{
				_selectedScrollArea.Value = 0f;
			}
			if (_gameController != null && _gameController.Model.State == GameState.Complete)
			{
				UnityEngine.Debug.LogException(new iAPShowInEnableMethodException(StackTraceUtility.ExtractStackTrace()));
			}
			DonDestroyMonoSingleton<InputManager>.Instance.DisableInputEvents(GetType());
		}

		protected override void OnDisable()
		{
			if (DonDestroyMonoSingleton<InputManager>.IsAlive)
			{
				DonDestroyMonoSingleton<InputManager>.Instance.EnableInputEvents(GetType());
			}
			base.OnDisable();
		}

		private void OnBtnCloseClick()
		{
			if (OnCloseEvent__BackingField != null)
			{
				OnCloseEvent__BackingField();
			}
			gameObject.SetActive(false);
		}

		private void OnNotEnoughGold()
		{
			Show();
		}

		private void OnNotEnoughGems()
		{
			if (_gameController != null && _gameController.Model.State == GameState.Complete)
			{
				UnityEngine.Debug.LogException(new iAPShowInShowMethodException(StackTraceUtility.ExtractStackTrace()));
			}
			gameObject.SetActive(true);
			OnBtnTicketsClick();
		}

		public void Show()
		{
			gameObject.SetActive(true);
			OnBtnCurrencyClick();
		}

		public void ShowAdsPanel(bool state)
		{
			if (_adsPanel != null)
			{
				_adsPanel.SetActive(state);
			}
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			tk2dUIScrollableArea selectedScrollArea = _selectedScrollArea;
			selectedScrollArea.Value = 0f;
			if (selectedScrollArea.contentContainer.transform.childCount > 0 || !(model is List<InAppPurchase>))
			{
				return;
			}
			List<InAppPurchase> list = model as List<InAppPurchase>;
			if (list.First().Category == InAppPurchase.InAppCategory.Coins)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(_iapVideoItemPrefab);
				gameObject.transform.parent = selectedScrollArea.contentContainer.transform;
				gameObject.transform.localPosition = new Vector3(0f, 0f, -0.01f);
				for (int i = 0; i < list.Count; i++)
				{
					gameObject = UnityEngine.Object.Instantiate(_iapItemPrefab);
					gameObject.transform.parent = selectedScrollArea.contentContainer.transform;
					gameObject.transform.localPosition = new Vector3((float)(i + 1) * 2.64f, 0f, -0.01f);
					gameObject.GetComponent<InAppItemController>().ApplyModel(list[i]);
				}
				selectedScrollArea.ContentLength = (float)list.Count * 2.64f;
			}
			else if (list.First().Category == InAppPurchase.InAppCategory.Gems)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(_gemsVideoItemPrefab);
				gameObject2.transform.parent = selectedScrollArea.contentContainer.transform;
				gameObject2.transform.localPosition = new Vector3(0f, 0f, -0.01f);
				for (int j = 0; j < list.Count; j++)
				{
					gameObject2 = UnityEngine.Object.Instantiate(_gemsItemPrefab);
					gameObject2.transform.parent = selectedScrollArea.contentContainer.transform;
					gameObject2.transform.localPosition = new Vector3((float)(j + 1) * 2.64f, 0f, -0.01f);
					gameObject2.GetComponent<InAppItemController>().ApplyModel(list[j]);
				}
				selectedScrollArea.ContentLength = (float)list.Count * 2.64f;
			}
			else if (list.First().Category == InAppPurchase.InAppCategory.Subscription)
			{
				for (int k = 0; k < list.Count; k++)
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate(_subscriptionItemPrefab);
					gameObject3.transform.parent = selectedScrollArea.contentContainer.transform;
					gameObject3.transform.localPosition = new Vector3((float)k * 5.38f, 0f, -0.01f);
					gameObject3.GetComponent<SubscriptionItemController>().ApplyModel(list[k]);
				}
				selectedScrollArea.ContentLength = (float)list.Count * 5.38f - 2.69f;
			}
		}

		public override void Close()
		{
			OnBtnCloseClick();
		}

		protected override void OnReleaseResource()
		{
			if (MonoSingleton<AppController>.IsAlive)
			{
				MonoSingleton<AppController>.Instance.Model.ShopModel.OnNotEnoughGoldEvent -= OnNotEnoughGold;
				MonoSingleton<AppController>.Instance.Model.ShopModel.OnNotEnoughGemsEvent -= OnNotEnoughGems;
			}
			base.OnReleaseResource();
		}

		private void OnBtnTicketsClick()
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabEvent__BackingField(InappTabEnum.Gems);
			}
			for (int i = 0; i < tabButtons.Count; i++)
			{
				tabButtons[i].IsOn = 2 == i;
			}
		}

		private void OnBtnCurrencyClick()
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabEvent__BackingField(InappTabEnum.Currency);
			}
			for (int i = 0; i < tabButtons.Count; i++)
			{
				tabButtons[i].IsOn = 1 == i;
			}
		}

		private void OnBtnSubscriptionsClick()
		{
			if (OnTabEvent__BackingField != null)
			{
				OnTabEvent__BackingField(InappTabEnum.Subscriptions);
			}
			for (int i = 0; i < tabButtons.Count; i++)
			{
				tabButtons[i].IsOn = 0 == i;
			}
		}

		public void OnTabActivate(InappTabEnum tab)
		{
			for (int i = 0; i < tabButtons.Count; i++)
			{
				tabButtons[i].IsOn = tab == (InappTabEnum)i;
			}
			_selectedScrollArea = _scrollAreas[(int)tab];
			_selectedScrollArea.gameObject.SetActive(true);
		}

		public void OnTabDisactivate(InappTabEnum tab)
		{
			tk2dUIScrollableArea tk2dUIScrollableArea2 = _scrollAreas[(int)tab];
			tk2dUIScrollableArea2.gameObject.SetActive(false);
			tk2dUIScrollableArea2.Value = 0f;
		}
	}
}

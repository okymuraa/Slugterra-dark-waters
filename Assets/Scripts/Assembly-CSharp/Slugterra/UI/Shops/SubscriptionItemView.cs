using System;
using System.Collections;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.InAppPurchases;
using AppsMinistry.UI;
using DG.Tweening;
using Slugterra.App;
using Slugterra.Domain.Shops;
using Slugterra.InAppPurchases;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class SubscriptionItemView : UIView
	{
		[SerializeField]
		private TextMesh _lblType;

		[SerializeField]
		private TextMesh _lblBondType;

		[SerializeField]
		private TextMesh _lblDescription;

		[SerializeField]
		private TextMesh _buttonAvailableText;

		[SerializeField]
		private TextMesh _buttonUnavailableText;

		[SerializeField]
		private GameObject _image;

		[SerializeField]
		private GameObject _button;

		[SerializeField]
		private GameObject _buttonUnavailable;

		[SerializeField]
		private GameObject _timer;

		[SerializeField]
		private GameObject _acquired;

		[SerializeField]
		private GameObject _buttonBuy;

		[SerializeField]
		private TextMesh _timerText;

		[SerializeField]
		private GameObject _saleBadge;

		[SerializeField]
		private GameObject _bestSellerBadge;

		[SerializeField]
		private TextMesh[] _deposit;

		[SerializeField]
		private TextMesh[] _depositPeriod;

		private string TEXT_DAYS = "d";

		private List<TextMesh> valueLabels;

		private InAppPurchase model;

		private DateTime _expirationTime = DateTime.MinValue;

		private Tween timerUpdateT;

		protected override void OnStart()
		{
			base.OnStart();
			TEXT_DAYS = MonoSingleton<Localization>.Instance.GetValueByKey("days");
		}

		private void OnEnable()
		{
			if (_expirationTime.CompareTo(DateTime.MinValue) > 0)
			{
				TimerUpdate(true);
			}
			RefreshState();
		}

		private void OnDisable()
		{
			TimerUpdate(false);
		}

		protected override void OnApplyModel(object _model)
		{
			model = _model as InAppPurchase;
			RefreshState();
		}

		public void RefreshState()
		{
			if (model != null)
			{
				InAppPurchase inAppPurchase = model;
				SubscriptionItem subscriptionItem = DonDestroyMonoSingleton<SubscriptionManager>.Instance.Subscriptions[inAppPurchase.ProductID];
				if (subscriptionItem != null)
				{
					_lblType.text = MonoSingleton<Localization>.Instance.GetValueByKey(subscriptionItem.Type);
					_lblBondType.text = MonoSingleton<Localization>.Instance.GetValueByKey(subscriptionItem.BondType);
					_lblDescription.text = MonoSingleton<Localization>.Instance.GetValueByKey(subscriptionItem.Description);
				}
				_buttonAvailableText.text = DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.GetFormattedPrice(string.Format("{0}", inAppPurchase.ProductID));
				_buttonUnavailableText.text = _buttonAvailableText.text;
				bool isBestSeller = model.IsBestSeller;
				bool flag = model.Sale != 0;
				if (_bestSellerBadge != null)
				{
					_bestSellerBadge.SetActive(false);
				}
				if (_saleBadge != null)
				{
					_saleBadge.SetActive(false);
				}
				if (_acquired != null)
				{
					_acquired.SetActive(false);
				}
				if (_buttonBuy != null)
				{
					_buttonBuy.SetActive(false);
				}
				if (isBestSeller)
				{
					_bestSellerBadge.SetActive(true);
				}
				if (flag)
				{
					_saleBadge.SetActive(true);
				}
				if (_timer != null)
				{
					_timer.SetActive(false);
				}
				if (subscriptionItem.State == SubscriptionItemState.AVAILABLE)
				{
					_buttonBuy.SetActive(true);
					_button.SetActive(true);
					_buttonUnavailable.SetActive(false);
					_expirationTime = DateTime.MinValue;
				}
				if (subscriptionItem.State == SubscriptionItemState.UNAVAILABLE || subscriptionItem.State == SubscriptionItemState.NONE)
				{
					_buttonBuy.SetActive(true);
					_button.SetActive(false);
					_buttonUnavailable.SetActive(true);
					_expirationTime = DateTime.MinValue;
				}
				if (MonoSingleton<AppController>.Instance.Model.ShopModel.AvailableMultiplier == 2)
				{
					_buttonBuy.SetActive(false);
					_expirationTime = subscriptionItem.ExpirationTime;
					int num = DateTime.Now.CompareTo(_expirationTime);
					_timer.SetActive(num < 0);
					_acquired.SetActive(num >= 0);
					TimerUpdate(true);
				}
			}
		}

		private IEnumerator TimerCoroutine()
		{
			int i = 0;
			Tween myTween = DOTween.To(() => i, (int x) =>
			{
				i = i;
			}, 1, 1f);
			myTween.SetLoops(-1);
			myTween.OnUpdate(() =>
			{
				Debug.Log("TWEEN!!!");
			});
			while (_timerText.gameObject.activeInHierarchy)
			{
				TimeSpan remains = _expirationTime.Subtract(DateTime.Now);
				_timerText.text = string.Format("{0:00}:{1:00}:{2:00}", remains.Hours, remains.Minutes, remains.Seconds);
				if (remains.Days > 1)
				{
					_timerText.text += string.Format("{0:0}{1}", remains.Days, TEXT_DAYS);
				}
				yield return myTween.WaitForElapsedLoops(1);
			}
		}

		private void TimerUpdate(bool isShow)
		{
			if (isShow)
			{
				if (timerUpdateT != null)
				{
					timerUpdateT.Kill();
				}
				int i = 0;
				timerUpdateT = DOTween.To(() => i, (int x) =>
				{
					i = i;
				}, 1, 1f);
				timerUpdateT.SetLoops(-1);
				timerUpdateT.OnUpdate(() =>
				{
					TimeSpan timeSpan = _expirationTime.Subtract(DateTime.Now);
					string text = string.Format("{3:0}{4} {0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Days, TEXT_DAYS);
					_timerText.text = text;
				});
			}
			else if (timerUpdateT != null)
			{
				timerUpdateT.Kill();
			}
		}
	}
}

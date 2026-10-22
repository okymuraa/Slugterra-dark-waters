using System;
using AppsMinistry.Core;
using AppsMinistry.Utils;
using DG.Tweening;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.InAppPurchases
{
	public class SubscriptionControllerSlug : SubscriptionController
	{
		private SubscriptionItemSlug _subscriptionItem;

		private SubscriptionContextSlug _activSubContext;

		private Tween waitEndSub;

		public override SubscriptionItem Item
		{
			get
			{
				return _subscriptionItem;
			}
		}

		public override void SetSubsciptionItem(SubscriptionItem subItem)
		{
			_subscriptionItem = (SubscriptionItemSlug)subItem;
		}

		public override void OnApplicationPause(bool paused)
		{
			if (paused)
			{
				WaitEndSub(false);
			}
			else
			{
				CheckEndSub();
			}
		}

		public override void Init()
		{
			InitSubscription();
			CheckEndSub();
		}

		public override void OnSubscriptionPurchased(int validThru)
		{
			ResolveSubscription();
			string sKU = _subscriptionItem.SKU;
			DateTime now = DateTime.Now;
			int durationDays = _subscriptionItem.DurationDays;
			int value = validThru - (int)WWWUtils.ConvertToTimestamp(now);
			PlayerPrefs.SetString("nameSub" + _subscriptionItem.SKU, sKU);
			PlayerPrefs.SetInt("periodAction" + _subscriptionItem.SKU, value);
			PlayerPrefs.SetInt("dateStartStamp" + _subscriptionItem.SKU, (int)WWWUtils.ConvertToTimestamp(now));
			PlayerPrefs.Save();
			InitSubscription();
			CheckEndSub();
		}

		private void InitSubscription()
		{
			string nameSub = PlayerPrefs.GetString("nameSub" + _subscriptionItem.SKU, string.Empty);
			_activSubContext = new SubscriptionContextSlug(_subscriptionItem, nameSub, WWWUtils.UnixTimeStampToDateTime(PlayerPrefs.GetInt("dateStartStamp" + _subscriptionItem.SKU, 0)), PlayerPrefs.GetInt("periodAction" + _subscriptionItem.SKU, 0));
			_activSubContext.sItem.ExpirationTime = _activSubContext.dateEnd;
			if (_activSubContext != null && _activSubContext.nameSub != string.Empty && DateTime.Now.CompareTo(_activSubContext.dateEnd) < 0)
			{
				_activSubContext.sItem.State = SubscriptionItemState.ACTIVATE;
				SetSlugAvailable(true);
			}
			else
			{
				_activSubContext.sItem.State = SubscriptionItemState.AVAILABLE;
			}
		}

		private void SetSlugAvailable(bool isAvailable)
		{
			if (isAvailable)
			{
				DonDestroyMonoSingleton<SubscriptionManager>.Instance.AddSubscribeBlaster(_subscriptionItem.SlugType);
			}
			else
			{
				DonDestroyMonoSingleton<SubscriptionManager>.Instance.RemoveSubscribeBlaster(_subscriptionItem.SlugType);
			}
		}

		private void CheckEndSub()
		{
			if (_activSubContext != null && _activSubContext.nameSub != string.Empty)
			{
				if (DateTime.Now.CompareTo(_activSubContext.dateEnd) < 0)
				{
					WaitEndSub(true);
					return;
				}
				ResetItem();
				ResolveSubscription();
			}
		}

		private void ResetItem()
		{
			SetSlugAvailable(false);
			Debug.Log("End SLug!!!!!!!!!!");
		}

		protected override void ResolveSubscription()
		{
			if (_activSubContext != null)
			{
				_activSubContext.sItem.State = SubscriptionItemState.AVAILABLE;
			}
			PlayerPrefs.SetString("nameSub" + _subscriptionItem.SKU, string.Empty);
			PlayerPrefs.SetInt("periodAction" + _subscriptionItem.SKU, 0);
			PlayerPrefs.SetInt("dateStartStamp" + _subscriptionItem.SKU, 0);
			PlayerPrefs.Save();
			InitSubscription();
			base.ResolveSubscription();
		}

		private void WaitEndSub(bool isStart)
		{
			if (isStart)
			{
				if (waitEndSub != null)
				{
					waitEndSub.Kill();
				}
				int i = 0;
				DateTime dateEndSub = _activSubContext.dateEnd;
				waitEndSub = DOTween.To(() => i, (int x) =>
				{
					i = i;
				}, 1, 1f);
				waitEndSub.SetLoops(-1);
				waitEndSub.OnUpdate(() =>
				{
					if (dateEndSub.CompareTo(DateTime.Now) <= 0)
					{
						waitEndSub.Kill();
						ResetItem();
						ResolveSubscription();
					}
				});
			}
			else if (waitEndSub != null)
			{
				waitEndSub.Kill();
			}
		}
	}
}

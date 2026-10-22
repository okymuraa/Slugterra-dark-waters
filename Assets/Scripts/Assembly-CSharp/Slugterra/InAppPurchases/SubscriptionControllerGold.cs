using System;
using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Utils;
using DG.Tweening;
using Slugterra.App;
using Slugterra.Domain.App;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.InAppPurchases
{
	public class SubscriptionControllerGold : SubscriptionController
	{
		private SubscriptionItemGold _subscriptionItem;

		private Wallet wallet;

		private SubscriptionContextGold _activSubContext;

		private Tween waitAddGoldT;

		public override SubscriptionItem Item
		{
			get
			{
				return _subscriptionItem;
			}
		}

		public SubscriptionContextGold ActiveSubContext
		{
			get
			{
				return _activSubContext;
			}
		}

		public override void SetSubsciptionItem(SubscriptionItem subItem)
		{
			_subscriptionItem = (SubscriptionItemGold)subItem;
		}

		public override void OnApplicationPause(bool paused)
		{
			if (paused)
			{
				WaitAddGold(false);
				return;
			}
			CheckMissedPay();
			CheckFuturePay();
		}

		public override void Init()
		{
			InitSubscription();
			CheckMissedPay();
			CheckFuturePay();
		}

		public override void OnSubscriptionPurchased(int validThru)
		{
			WaitAddGold(false);
			ResolveSubscription();
			string sKU = _subscriptionItem.SKU;
			DateTime now = DateTime.Now;
			int durationDays = _subscriptionItem.DurationDays;
			int value = 0;
			int num = validThru - (int)WWWUtils.ConvertToTimestamp(now);
			Debug.LogFormat("SubscriptionControllerGold::OnSubscriptionPurchased validThru {0} ({1}); Date Start {2} ({3}); PeriodSub {4}", validThru, WWWUtils.UnixTimeStampToDateTime(validThru), (int)WWWUtils.ConvertToTimestamp(now), now, num);
			PlayerPrefs.SetString("nameSub" + _subscriptionItem.SKU, sKU);
			PlayerPrefs.SetInt("periodAction" + _subscriptionItem.SKU, num);
			PlayerPrefs.SetInt("countPay" + _subscriptionItem.SKU, durationDays);
			PlayerPrefs.SetInt("donePay" + _subscriptionItem.SKU, value);
			PlayerPrefs.SetInt("dateStartStamp" + _subscriptionItem.SKU, (int)WWWUtils.ConvertToTimestamp(now));
			PlayerPrefs.Save();
			AddGold(int.Parse(_subscriptionItem.GoldDaily));
			InitSubscription();
			CheckFuturePay();
		}

		private void InitSubscription()
		{
			string nameSub = PlayerPrefs.GetString("nameSub" + _subscriptionItem.SKU, string.Empty);
			_activSubContext = new SubscriptionContextGold(_subscriptionItem, nameSub, WWWUtils.UnixTimeStampToDateTime(PlayerPrefs.GetInt("dateStartStamp" + _subscriptionItem.SKU, 0)), PlayerPrefs.GetInt("periodAction" + _subscriptionItem.SKU, 0), PlayerPrefs.GetInt("countPay" + _subscriptionItem.SKU, 0), PlayerPrefs.GetInt("donePay" + _subscriptionItem.SKU, 0));
			_activSubContext.sItem.ExpirationTime = _activSubContext.dateEnd;
			if (_activSubContext.donePay < _activSubContext.countPay)
			{
				_activSubContext.sItem.State = SubscriptionItemState.ACTIVATE;
			}
			else
			{
				_activSubContext.sItem.State = SubscriptionItemState.AVAILABLE;
			}
		}

		private void CheckMissedPay()
		{
			if (_activSubContext == null || !(_activSubContext.nameSub != string.Empty))
			{
				return;
			}
			if (_activSubContext.donePay < _activSubContext.countPay)
			{
				int num = _activSubContext.countPay - ((int)(_activSubContext.dateEnd - DateTime.Now).TotalSeconds / _activSubContext.periodPay + 1) - _activSubContext.donePay;
				for (int i = 0; i < num; i++)
				{
					AddGold(int.Parse(((SubscriptionItemGold)_activSubContext.sItem).GoldDaily));
					_activSubContext.donePay++;
					PlayerPrefs.SetInt("donePay" + _subscriptionItem.SKU, _activSubContext.donePay);
					PlayerPrefs.Save();
				}
			}
			else
			{
				ResolveSubscription();
			}
		}

		private void CheckFuturePay()
		{
			if (_activSubContext != null && _activSubContext.nameSub != string.Empty)
			{
				if (_activSubContext.donePay < _activSubContext.countPay)
				{
					WaitAddGold(true);
				}
				else
				{
					ResolveSubscription();
				}
			}
		}

		private void AddGold(int count)
		{
			if (wallet == null)
			{
				wallet = MonoSingleton<AppController>.Instance.Model.Wallet;
			}
			wallet.Gold += count;
		}

		protected override void ResolveSubscription()
		{
			if (_activSubContext != null)
			{
				_activSubContext.sItem.State = SubscriptionItemState.AVAILABLE;
			}
			PlayerPrefs.SetString("nameSub" + _subscriptionItem.SKU, string.Empty);
			PlayerPrefs.SetInt("periodAction" + _subscriptionItem.SKU, 0);
			PlayerPrefs.SetInt("countPay" + _subscriptionItem.SKU, 0);
			PlayerPrefs.SetInt("donePay" + _subscriptionItem.SKU, 0);
			PlayerPrefs.SetInt("dateStartStamp" + _subscriptionItem.SKU, 0);
			PlayerPrefs.Save();
			InitSubscription();
			base.ResolveSubscription();
		}

		private IEnumerator WaitToAddGold()
		{
			int periodToNextPay = Convert.ToInt32(_activSubContext.dateStart.AddSeconds(Convert.ToDouble((_activSubContext.donePay + 1) * _activSubContext.periodPay)).Subtract(DateTime.Now).TotalSeconds);
			yield return new WaitForSeconds(periodToNextPay);
			AddGold(int.Parse(((SubscriptionItemGold)_activSubContext.sItem).GoldDaily));
			_activSubContext.donePay++;
			PlayerPrefs.SetInt("donePay" + _subscriptionItem.SKU, _activSubContext.donePay);
			PlayerPrefs.Save();
			CheckFuturePay();
		}

		private void WaitAddGold(bool isStart)
		{
			if (isStart)
			{
				if (waitAddGoldT != null)
				{
					waitAddGoldT.Kill();
				}
				int i = 0;
				int num = Convert.ToInt32(_activSubContext.dateStart.AddSeconds(Convert.ToDouble((_activSubContext.donePay + 1) * _activSubContext.periodPay)).Subtract(DateTime.Now).TotalSeconds);
				DateTime dateAddGold = DateTime.Now.AddSeconds(num);
				waitAddGoldT = DOTween.To(() => i, (int x) =>
				{
					i = i;
				}, 1, 1f);
				waitAddGoldT.SetLoops(-1);
				waitAddGoldT.OnUpdate(() =>
				{
					if (dateAddGold.CompareTo(DateTime.Now) <= 0)
					{
						AddGold(int.Parse(((SubscriptionItemGold)_activSubContext.sItem).GoldDaily));
						_activSubContext.donePay++;
						PlayerPrefs.SetInt("donePay" + _subscriptionItem.SKU, _activSubContext.donePay);
						PlayerPrefs.Save();
						waitAddGoldT.Kill();
						CheckFuturePay();
					}
				});
			}
			else if (waitAddGoldT != null)
			{
				waitAddGoldT.Kill();
			}
		}
	}
}

using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Domain.Shops;

namespace Slugterra.InAppPurchases
{
	[AppSingleton]
	public class SubscriptionManager : DonDestroyMonoSingleton<SubscriptionManager>
	{
		private Dictionary<string, SubscriptionItem> _subscriptions = new Dictionary<string, SubscriptionItem>();

		private Dictionary<string, SubscriptionController> _subscriptionControllers = new Dictionary<string, SubscriptionController>();

		private List<string> _subscribedBlasters = new List<string>();

		private DateTime dateSub = default(DateTime);

		public Dictionary<string, SubscriptionItem> Subscriptions
		{
			get
			{
				return _subscriptions;
			}
		}

		public Dictionary<string, SubscriptionController> SubscriptionControllers
		{
			get
			{
				return _subscriptionControllers;
			}
		}

		public List<string> SubscribedBlasters
		{
			get
			{
				return _subscribedBlasters;
			}
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			FillItems();
		}

		private void OnApplicationPause(bool paused)
		{
			foreach (SubscriptionController value in _subscriptionControllers.Values)
			{
				value.OnApplicationPause(paused);
			}
		}

		public void Init()
		{
			InitSubscription();
		}

		private void FillItems()
		{
			_subscriptions.Clear();
			_subscriptions.Add("com.appsministry.slugterra.2x_multiplier", new SubscriptionItemMultiplier("2_x_multiplier_title", "multiplier", "2_x_multiplier_desc3", "com.appsministry.slugterra.2x_multiplier", 90));
			_subscriptions["com.appsministry.slugterra.2x_multiplier"].State = SubscriptionItemState.AVAILABLE;
			foreach (string key in _subscriptions.Keys)
			{
				if (_subscriptions[key] is SubscriptionItemGold)
				{
					SubscriptionControllerGold subscriptionControllerGold = new SubscriptionControllerGold();
					subscriptionControllerGold.SetSubsciptionItem(_subscriptions[key]);
					_subscriptionControllers.Add(key, subscriptionControllerGold);
				}
				if (_subscriptions[key] is SubscriptionItemSlug)
				{
					SubscriptionControllerSlug subscriptionControllerSlug = new SubscriptionControllerSlug();
					subscriptionControllerSlug.SetSubsciptionItem(_subscriptions[key]);
					_subscriptionControllers.Add(key, subscriptionControllerSlug);
				}
				if (_subscriptions[key] is SubscriptionItemMultiplier)
				{
					SubscriptionControllerMultiplier subscriptionControllerMultiplier = new SubscriptionControllerMultiplier();
					subscriptionControllerMultiplier.SetSubsciptionItem(_subscriptions[key]);
					_subscriptionControllers.Add(key, subscriptionControllerMultiplier);
				}
			}
		}

		public void OnSubscriptionPurchased(string SKU, int validThru)
		{
			dateSub = WWWUtils.UnixTimeStampToDateTime(validThru);
			if (SKU != "com.appsministry.slugterra.2x_multiplier")
			{
				if (DateTime.UtcNow.CompareTo(WWWUtils.UnixTimeStampToDateTime(validThru)) < 0 && _subscriptions.ContainsKey(SKU) && _subscriptionControllers.ContainsKey(SKU))
				{
					SubscriptionController subscriptionController = _subscriptionControllers[SKU];
					subscriptionController.OnSubscriptionPurchased(validThru);
					UpadteSubStates();
				}
			}
			else if (_subscriptions.ContainsKey(SKU) && _subscriptionControllers.ContainsKey(SKU))
			{
				SubscriptionController subscriptionController2 = _subscriptionControllers[SKU];
				subscriptionController2.Item.State = SubscriptionItemState.ACTIVATE;
				MonoSingleton<AppController>.Instance.Model.ShopModel.AvailableMultiplier = 2;
			}
		}

		private void InitSubscription()
		{
			foreach (SubscriptionController value in _subscriptionControllers.Values)
			{
				value.OnResolveSubscription += UpadteSubStates;
				value.Init();
			}
			UpadteSubStates();
		}

		private void UpadteSubStates()
		{
			bool flag = false;
			foreach (SubscriptionController value in _subscriptionControllers.Values)
			{
				if (value.Item != null && value.Item.State == SubscriptionItemState.ACTIVATE)
				{
					flag = true;
				}
			}
			foreach (SubscriptionController value2 in _subscriptionControllers.Values)
			{
				if (value2.Item is SubscriptionItemGold && value2.Item.State == SubscriptionItemState.AVAILABLE && flag)
				{
					value2.Item.State = SubscriptionItemState.UNAVAILABLE;
				}
				if (value2.Item is SubscriptionItemGold && value2.Item.State == SubscriptionItemState.UNAVAILABLE && !flag)
				{
					value2.Item.State = SubscriptionItemState.AVAILABLE;
				}
				if (value2.Item is SubscriptionItemMultiplier)
				{
					value2.Item.State = ((MonoSingleton<AppController>.Instance.Model.ShopModel.AvailableMultiplier != 2) ? SubscriptionItemState.AVAILABLE : SubscriptionItemState.ACTIVATE);
				}
			}
		}

		public void AddSubscribeBlaster(string blasterType)
		{
			if (!_subscribedBlasters.Contains(blasterType))
			{
				_subscribedBlasters.Add(blasterType);
			}
		}

		public void RemoveSubscribeBlaster(string blasterType)
		{
			if (_subscribedBlasters.Contains(blasterType))
			{
				_subscribedBlasters.Remove(blasterType);
			}
		}
	}
}

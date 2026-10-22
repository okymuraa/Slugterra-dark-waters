using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.InAppPurchases;
using UnityEngine;

namespace AppsMinistry.InAppPurchases
{
	[AppSingleton]
	public abstract class PurchaseContext<T> : DonDestroyMonoSingleton<T>, IPurchaseContext where T : PurchaseContext<T>
	{
		protected PurchaseService _strategy;

		private bool _isPurchasing;

		private float _purchasingDelay = 15f;

		public GameObject GameObject => gameObject;

		public event Action<string> OnSuccessPurchase;

		protected abstract List<InAppPurchase> GetInApps();

		protected override void OnCreate()
		{
			_strategy = PurchaseService.Create<GoogleMarketStrategy_ANP>(this);
			_strategy.OnUpdateSuccessPurchase += UpdateSuccessPurchase;
			_strategy.Initialize();
		}

		public List<string> GetProductsIds(InAppMarket market)
		{
			return InApps.ALL_INAPPS.Select((InAppPurchase obj) => obj.ProductID).ToList();
		}

		public void PurchaseProduct(string productId, Action OnSuccess = null, Action OnFail = null, Action OnCancel = null)
		{
			if (!_isPurchasing)
			{
				_isPurchasing = true;
				StartCoroutine(CoroutinePurchasingDelay(_purchasingDelay));
				int availableMultiplier = MonoSingleton<AppController>.Instance.Model.ShopModel.AvailableMultiplier;
				if (!DonDestroyMonoSingleton<GameServiceContext>.Instance.IsPlayerAuthenticated())
				{
					DonDestroyMonoSingleton<GameServiceContext>.Instance.AuthenticateLocalPlayer();
				}
				_strategy.PurchaseProduct(productId, OnSuccess, OnFail, OnCancel);
			}
		}

		private IEnumerator CoroutinePurchasingDelay(float time)
		{
			yield return new WaitForSeconds(time);
			_isPurchasing = false;
		}

		public void RestorePurchases()
		{
			_strategy.RestorePurchases();
		}

		public string GetFormattedPrice(string productID)
		{
			string valueByKey = MonoSingleton<Localization>.Instance.GetValueByKey(productID);
			string formattedPrice = _strategy.GetFormattedPrice(productID);
			if (!string.IsNullOrEmpty(formattedPrice))
			{
				PlayerPrefs.SetString(productID, formattedPrice);
			}
			valueByKey = (PlayerPrefsSystem.HasKey(productID) ? PlayerPrefs.GetString(productID) : MonoSingleton<Localization>.Instance.GetValueByKey(productID));
			if (valueByKey.Contains("com.appsministry"))
			{
				PlayerPrefs.DeleteKey(productID);
				valueByKey = MonoSingleton<Localization>.Instance.GetValueByKey(productID);
			}
			return valueByKey;
		}

		public string GetCountryCode()
		{
			return _strategy.countryCode;
		}

		public bool IsPurchasing()
		{
			return _strategy.isPurchasing;
		}

		private void UpdateSuccessPurchase(string productId, int validThru)
		{
			_isPurchasing = false;
			UnityEngine.Debug.Log("PurchaseContext::UpdateSuccessPurchase");
			InApps.ALL_INAPPS.Find((InAppPurchase obj) => obj.ProductID == productId).OnSuccessBuy(validThru);
			if (this.OnSuccessPurchase != null)
			{
				this.OnSuccessPurchase(productId);
			}
		}
	}
}

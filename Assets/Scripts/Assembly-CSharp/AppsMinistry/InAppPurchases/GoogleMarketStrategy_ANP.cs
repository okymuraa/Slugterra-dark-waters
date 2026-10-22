using System;
using System.Collections.Generic;
using AppsMinistry.Core;
using AppsMinistry.Plugins;
using AppsMinistry.Utils;
using Slugterra.InAppPurchases;
using UnityEngine;

namespace AppsMinistry.InAppPurchases
{
	public class GoogleMarketStrategy_ANP : PurchaseService
	{
		private static bool _isInitialized;

		private bool _isPurchasing;

		private Action _OnSuccess;

		private bool _isProductListRecieved = true;

		private bool _isRestoringPurchase;

		private bool _isValidating;

		private bool _isRequestProductInProgress;

		public override bool isPurchasing
		{
			get
			{
				return _isPurchasing;
			}
			protected set
			{
				_isPurchasing = value;
				if (!_isPurchasing)
				{
					_OnSuccess = null;
				}
			}
		}

		public override void Initialize()
		{
			List<string> productsIds = _context.GetProductsIds(InAppMarket.GoogleMarket);
			foreach (string item in productsIds)
			{
				AndroidInAppPurchaseManager.Client.AddProduct(item);
			}
			AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;
			AndroidInAppPurchaseManager.ActionProductConsumed += OnProductConsumed;
			AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingSetupFinished;
			AndroidInAppPurchaseManager.Client.Connect("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwalJH+sXbT8++wOUgsj61xw5ksRag/9cDBsgqvejh7RpCaUsO5SMaw73Stxe3pm/DAcfEOUph8ueGxgeV08gaxjhhGG5Qquy4JpTKD1PuBvbWny+kvbzX5Icu4nfSVs4/vyNc1cQ2eqNBU+hArPa7v6FuDiIblNvBT381Z2N6P3MdeohJdNTWiHB/PC2GT1FHPO77u4Ip66aVoAOeKCFOlhC6y76hdGnfvL6VB7MBoqQX9At+71LTTbmAlJI7yWvXQf6UcxMR653j8KjHuvZgGiarAMw/Nwv9S8KqG8G1keHpN+LR9tOY8Oa2zm7dTInYlf38MHR/scovXm/gOeA0QIDAQAB");
			_isRestoringPurchase = false;
			MonoSingleton<WWWUtils>.Instance.InitializeCountryCode(() =>
			{
				countryCode = MonoSingleton<WWWUtils>.Instance.CountryCode;
			});
		}

		public override void PurchaseProduct(string productId, Action OnSuccess, Action OnFail = null, Action OnCancel = null)
		{
			isPurchasing = true;
			_OnSuccess = OnSuccess;
			AndroidInAppPurchaseManager.Client.Purchase(productId);
		}

		public override void RestorePurchases()
		{
			_isRestoringPurchase = true;
			AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetriveProductsFinised;
			RequestProductData();
		}

		public override string GetFormattedPrice(string productID)
		{
			if (_isProductListRecieved)
			{
				GoogleProductTemplate productDetails = AndroidInAppPurchaseManager.Client.Inventory.GetProductDetails(productID);
				if (productDetails != null)
				{
					return productDetails.LocalizedPrice;
				}
			}
			else
			{
				RequestProductData();
			}
			return productID;
		}

		public override void UpdateSuccessPurchase(string productId, int validThru = -1)
		{
			RaiseUpdateSuccessPurchase(productId, validThru);
			if (_OnSuccess != null)
			{
				_OnSuccess();
			}
		}

		private void RequestProductData()
		{
			if (!_isRequestProductInProgress && _isInitialized)
			{
				_isRequestProductInProgress = true;
				Debug.Log("GoogleMarketStrategy_ANP::RequestProductData ");
				AndroidInAppPurchaseManager.Client.RetrieveProducDetails();
			}
		}

		private void OnProductPurchased(BillingResult result)
		{
			if (result.isSuccess)
			{
				OnPurchaseSucceededEvent(result.purchase);
				return;
			}
			Debug.LogError("GoogleMarketStrategy_ANP::OnProductPurchased Failed " + result.response + " " + result.message);
			OnPurchaseFailedEvent(result.message);
		}

		private void OnPurchaseSucceededEvent(GooglePurchaseTemplate purchase)
		{
			Debug.Log("GoogleMarketStrategy_ANP::OnPurchaseSucceededEvent  " + purchase);
			InAppPurchase inAppPurchase = InApps.ALL_INAPPS.Find((InAppPurchase obj) => obj.ProductID == purchase.SKU);
			if (inAppPurchase == null)
			{
				return;
			}
			float productPrice = 0f;
			GoogleProductTemplate googleProductTemplate = null;
			if (_isProductListRecieved && AndroidInAppPurchaseManager.Client.Inventory != null)
			{
				googleProductTemplate = AndroidInAppPurchaseManager.Client.Inventory.GetProductDetails(purchase.SKU);
				if (googleProductTemplate != null)
				{
					productPrice = googleProductTemplate.Price;
				}
			}
			if (inAppPurchase.InAppType == InAppType.Consumable)
			{
				AndroidInAppPurchaseManager.Client.Consume(purchase.SKU);
				return;
			}
			DonDestroyMonoSingleton<AnalyticsManager>.Instance.AppsFlyerValidatePurchase("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwalJH+sXbT8++wOUgsj61xw5ksRag/9cDBsgqvejh7RpCaUsO5SMaw73Stxe3pm/DAcfEOUph8ueGxgeV08gaxjhhGG5Qquy4JpTKD1PuBvbWny+kvbzX5Icu4nfSVs4/vyNc1cQ2eqNBU+hArPa7v6FuDiIblNvBT381Z2N6P3MdeohJdNTWiHB/PC2GT1FHPO77u4Ip66aVoAOeKCFOlhC6y76hdGnfvL6VB7MBoqQX9At+71LTTbmAlJI7yWvXQf6UcxMR653j8KjHuvZgGiarAMw/Nwv9S8KqG8G1keHpN+LR9tOY8Oa2zm7dTInYlf38MHR/scovXm/gOeA0QIDAQAB", purchase.OriginalJson, purchase.Signature, googleProductTemplate.Price.ToString(), googleProductTemplate.PriceCurrencyCode, null, () =>
			{
				UpdateSuccessPurchase(purchase.SKU, -1);
				AnalyticsManager.LogInApp(purchase.OrderId, purchase.PackageName, purchase.SKU, productPrice, 1, "BuyServerSuccess", (googleProductTemplate == null) ? string.Empty : googleProductTemplate.PriceCurrencyCode);
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.TrackPurchase("Purchase", purchase.SKU, "success");
				isPurchasing = false;
			}, (string error) =>
			{
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.TrackPurchase("Purchase", purchase.SKU, "fail");
				isPurchasing = false;
			});
		}

		private void OnPurchaseFailedEvent(string error)
		{
			Debug.Log("GoogleMarketStrategy_ANP::OnPurchaseFailedEvent  " + error);
			isPurchasing = false;
		}

		private void OnProductConsumed(BillingResult result)
		{
			if (result.IsSuccess)
			{
				OnConsumePurchaseSucceededEvent(result.Purchase);
				return;
			}
			Debug.LogError("GoogleMarketStrategy_ANP::OnProductConsumed  Failed " + result.Response + " " + result.Message);
			OnConsumePurchaseFailedEvent(result.Message);
		}

		private void OnConsumePurchaseSucceededEvent(GooglePurchaseTemplate purchase)
		{
			Debug.Log("GoogleMarketStrategy_ANP::OnConsumePurchaseSucceededEvent  " + purchase);
			float productPrice = 0f;
			GoogleProductTemplate googleProductTemplate = null;
			if (_isProductListRecieved && AndroidInAppPurchaseManager.Client.Inventory != null)
			{
				googleProductTemplate = AndroidInAppPurchaseManager.Client.Inventory.GetProductDetails(purchase.SKU);
				if (googleProductTemplate != null)
				{
					productPrice = googleProductTemplate.Price;
				}
			}
			DonDestroyMonoSingleton<AnalyticsManager>.Instance.AppsFlyerValidatePurchase("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwalJH+sXbT8++wOUgsj61xw5ksRag/9cDBsgqvejh7RpCaUsO5SMaw73Stxe3pm/DAcfEOUph8ueGxgeV08gaxjhhGG5Qquy4JpTKD1PuBvbWny+kvbzX5Icu4nfSVs4/vyNc1cQ2eqNBU+hArPa7v6FuDiIblNvBT381Z2N6P3MdeohJdNTWiHB/PC2GT1FHPO77u4Ip66aVoAOeKCFOlhC6y76hdGnfvL6VB7MBoqQX9At+71LTTbmAlJI7yWvXQf6UcxMR653j8KjHuvZgGiarAMw/Nwv9S8KqG8G1keHpN+LR9tOY8Oa2zm7dTInYlf38MHR/scovXm/gOeA0QIDAQAB", purchase.OriginalJson, purchase.Signature, googleProductTemplate.Price.ToString(), googleProductTemplate.PriceCurrencyCode, null, () =>
			{
				UpdateSuccessPurchase(purchase.SKU, -1);
				AnalyticsManager.LogInApp(purchase.OrderId, purchase.PackageName, purchase.SKU, productPrice, 1, "BuyServerSuccess", (googleProductTemplate == null) ? string.Empty : googleProductTemplate.PriceCurrencyCode);
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.TrackPurchase("Purchase", purchase.SKU, "success");
				isPurchasing = false;
			}, (string error) =>
			{
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.TrackPurchase("Purchase", purchase.SKU, "fail");
				isPurchasing = false;
			});
		}

		private void OnConsumePurchaseFailedEvent(string error)
		{
			Debug.LogError("GoogleMarketStrategy_ANP::OnConsumePurchaseFailedEvent  " + error);
			isPurchasing = false;
		}

		private void OnRetriveProductsFinised(BillingResult result)
		{
			AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= OnRetriveProductsFinised;
			if (result.IsSuccess)
			{
				_isInitialized = true;
				OnQueryInventorySucceededEvent(AndroidInAppPurchaseManager.Client.Inventory.Purchases);
				return;
			}
			Debug.LogError("GoogleMarketStrategy_ANP::OnRetriveProductsFinised  Failed " + result.Response + " " + result.Message);
			OnQueryInventoryFailedEvent(result.Message);
		}

		private void OnBillingSetupFinished(BillingResult result)
		{
			if (result.IsSuccess)
			{
				_isInitialized = true;
				Debug.Log("GoogleMarketStrategy_ANP::OnBillingSetupFinished ");
				AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetriveProductsFinised;
				RequestProductData();
			}
			else
			{
				Debug.LogError("GoogleMarketStrategy_ANP::OnBillingSetupFinished  Failed " + result.Response + " " + result.Message);
			}
		}

		private void OnQueryInventorySucceededEvent(List<GooglePurchaseTemplate> purchases)
		{
			_isProductListRecieved = true;
			Debug.Log("GoogleMarketStrategy_ANP::OnQueryInventorySucceededEvent  " + string.Format("total purchases: {0}", purchases.Count));
			int itemsRestored = 0;
			bool flag = false;
			foreach (GooglePurchaseTemplate purchase in purchases)
			{
				if (purchase.State != GooglePurchaseState.PURCHASED)
				{
					continue;
				}
				InAppPurchase inAppPurchase = InApps.ALL_INAPPS.Find((InAppPurchase obj) => obj.ProductID == purchase.SKU);
				if (inAppPurchase == null)
				{
					continue;
				}
				string text = string.Format("SKU {0} orderId: {1} state: {2} token: {3} time: {4} packageName: {5} signature: {6}", purchase.SKU, purchase.OrderId, purchase.State, purchase.Token, purchase.Time, purchase.PackageName, purchase.Signature);
				if (inAppPurchase.InAppType == InAppType.Consumable)
				{
					if (!flag)
					{
						flag = true;
						AndroidInAppPurchaseManager.Client.Consume(purchase.SKU);
					}
					continue;
				}
				float productPrice = 0f;
				GoogleProductTemplate googleProductTemplate = null;
				if (_isProductListRecieved && AndroidInAppPurchaseManager.Client.Inventory != null)
				{
					googleProductTemplate = AndroidInAppPurchaseManager.Client.Inventory.GetProductDetails(purchase.SKU);
					if (googleProductTemplate != null)
					{
						productPrice = googleProductTemplate.Price;
					}
				}
				DonDestroyMonoSingleton<AnalyticsManager>.Instance.AppsFlyerValidatePurchase("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwalJH+sXbT8++wOUgsj61xw5ksRag/9cDBsgqvejh7RpCaUsO5SMaw73Stxe3pm/DAcfEOUph8ueGxgeV08gaxjhhGG5Qquy4JpTKD1PuBvbWny+kvbzX5Icu4nfSVs4/vyNc1cQ2eqNBU+hArPa7v6FuDiIblNvBT381Z2N6P3MdeohJdNTWiHB/PC2GT1FHPO77u4Ip66aVoAOeKCFOlhC6y76hdGnfvL6VB7MBoqQX9At+71LTTbmAlJI7yWvXQf6UcxMR653j8KjHuvZgGiarAMw/Nwv9S8KqG8G1keHpN+LR9tOY8Oa2zm7dTInYlf38MHR/scovXm/gOeA0QIDAQAB", purchase.OriginalJson, purchase.Signature, googleProductTemplate.Price.ToString(), googleProductTemplate.PriceCurrencyCode, null, () =>
				{
					itemsRestored++;
					UpdateSuccessPurchase(purchase.SKU, -1);
					AnalyticsManager.LogInApp(purchase.OrderId, purchase.PackageName, purchase.SKU, productPrice, 1, "RestoredPurchase", (googleProductTemplate == null) ? string.Empty : googleProductTemplate.PriceCurrencyCode);
					DonDestroyMonoSingleton<AnalyticsManager>.Instance.TrackPurchase("Purchase", purchase.SKU, "restore success");
					isPurchasing = false;
				}, (string error) =>
				{
					DonDestroyMonoSingleton<AnalyticsManager>.Instance.TrackPurchase("Purchase", purchase.SKU, "restore fail");
					isPurchasing = false;
				});
			}
			Debug.Log("GoogleMarketStrategy_ANP::OnQueryInventorySucceededEvent Restored " + itemsRestored);
			if (_isRestoringPurchase)
			{
				if (itemsRestored == 0)
				{
					AndroidMessage.Create(string.Empty, Localization.GetValue("no_items_for_restored", "Unable to restore any purchases"));
				}
				else
				{
					string message = string.Format("{0} {1}", itemsRestored, Localization.GetValue("items_restored", "purchase(s) restored"));
					AndroidMessage.Create(string.Empty, message);
				}
				_isRestoringPurchase = false;
			}
			_isRequestProductInProgress = false;
		}

		private void OnQueryInventoryFailedEvent(string error)
		{
			_isRestoringPurchase = false;
			Debug.LogError("GoogleMarketStrategy_ANP::OnQueryInventoryFailedEvent " + error);
			_isProductListRecieved = false;
		}
	}
}

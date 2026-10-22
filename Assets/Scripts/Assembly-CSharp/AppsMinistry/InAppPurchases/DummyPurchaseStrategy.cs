using System;
using AppsMinistry.Core;
using AppsMinistry.Utils;

namespace AppsMinistry.InAppPurchases
{
	public class DummyPurchaseStrategy : PurchaseService
	{
		private static bool _isInitialized;

		private bool _isPurchasing;

		private Action _OnSuccess;

		private bool _isProductListRecieved = true;

		private bool _isRestoringPurchase;

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
		}

		public override void RestorePurchases()
		{
			_isRestoringPurchase = true;
		}

		public override string GetFormattedPrice(string productID)
		{
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
	}
}

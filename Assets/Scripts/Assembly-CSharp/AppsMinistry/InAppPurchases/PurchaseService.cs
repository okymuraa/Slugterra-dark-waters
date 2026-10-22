using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace AppsMinistry.InAppPurchases
{
	public abstract class PurchaseService : MonoBehaviour
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<string, int> OnUpdateSuccessPurchase__BackingField;

		protected IPurchaseContext _context;

		[SerializeField]
		private string _countryCode = string.Empty;

		public string countryCode
		{
			get
			{
				return _countryCode;
			}
			protected set
			{
				_countryCode = value;
			}
		}

		public abstract bool isPurchasing { get; protected set; }

		public event Action<string, int> OnUpdateSuccessPurchase
		{
			add
			{
				Action<string, int> action = OnUpdateSuccessPurchase__BackingField;
				Action<string, int> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUpdateSuccessPurchase__BackingField, (Action<string, int>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<string, int> action = OnUpdateSuccessPurchase__BackingField;
				Action<string, int> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnUpdateSuccessPurchase__BackingField, (Action<string, int>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		public static T Create<T>(IPurchaseContext context) where T : PurchaseService
		{
			T val = context.GameObject.AddComponent<T>();
			val._context = context;
			return val;
		}

		public abstract void Initialize();

		public abstract void PurchaseProduct(string productId, Action OnSuccess, Action OnFail = null, Action OnCancel = null);

		public abstract void RestorePurchases();

		public abstract string GetFormattedPrice(string productID);

		public abstract void UpdateSuccessPurchase(string productId, int validThru);

		protected void RaiseUpdateSuccessPurchase(string productId, int validThru)
		{
			if (OnUpdateSuccessPurchase__BackingField != null)
			{
				OnUpdateSuccessPurchase__BackingField(productId, validThru);
			}
		}
	}
}

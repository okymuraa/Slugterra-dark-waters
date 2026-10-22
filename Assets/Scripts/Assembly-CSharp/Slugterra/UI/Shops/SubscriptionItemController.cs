using System;
using AppsMinistry.Core;
using AppsMinistry.Domain.Observer;
using AppsMinistry.InAppPurchases;
using AppsMinistry.UI;
using Slugterra.InAppPurchases;

namespace Slugterra.UI.Shops
{
	public class SubscriptionItemController : UIController<SubscriptionItemView>, IObserver
	{
		private InAppPurchase _model;

		private Action OnSuccess;

		protected override void _OnEnable()
		{
			OnSuccess = (Action)Delegate.Combine(OnSuccess, new Action(OnSuccessEvent));
			if (_model == null)
			{
			}
		}

		protected override void _OnDisable()
		{
			OnSuccess = (Action)Delegate.Remove(OnSuccess, new Action(OnSuccessEvent));
			if (_model == null)
			{
			}
		}

		public void ApplyModel(InAppPurchase model)
		{
			View.ApplyModel(model);
			_model = model;
		}

		private void OnBuy()
		{
			DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.PurchaseProduct(_model.ProductID, OnSuccess);
			View.RefreshState();
		}

		private void OnSuccessEvent()
		{
			View.RefreshState();
		}

		public void OnObjectChanged(object observable)
		{
			if (observable is InAppPurchase)
			{
				ApplyModel(observable as InAppPurchase);
			}
		}
	}
}

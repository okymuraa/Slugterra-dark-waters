using System;
using AppsMinistry.UI;
using Slugterra.UI.Shops;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupUseRecommendedController : UIController<PopupUseRecommendedView>
	{
		private EquipStoreView _equipView;

		protected override void OnCreate()
		{
			base.OnCreate();
			_equipView = UnityEngine.Object.FindObjectOfType<EquipStoreView>();
			gameObject.SetActive(false);
		}

		private void OnEnable()
		{
			PopupUseRecommendedView view = View;
			view.OnBtnContinueClick = (Action)Delegate.Combine(view.OnBtnContinueClick, new Action(OnBtnContinueHandler));
			PopupUseRecommendedView view2 = View;
			view2.OnBtnCheckoutClick = (Action)Delegate.Combine(view2.OnBtnCheckoutClick, new Action(OnBtnCheckoutHandler));
		}

		private void OnDisable()
		{
			PopupUseRecommendedView view = View;
			view.OnBtnContinueClick = (Action)Delegate.Remove(view.OnBtnContinueClick, new Action(OnBtnContinueHandler));
			PopupUseRecommendedView view2 = View;
			view2.OnBtnCheckoutClick = (Action)Delegate.Remove(view2.OnBtnCheckoutClick, new Action(OnBtnCheckoutHandler));
		}

		private void OnBtnContinueHandler()
		{
			_equipView.SendMessage("OnBtnStartClick");
			View.Close();
		}

		private void OnBtnCheckoutHandler()
		{
			_equipView.SendMessage("OnBtnRecommendationsClick");
			View.Close();
		}
	}
}

using AppsMinistry.Core;
using AppsMinistry.InAppPurchases;
using AppsMinistry.UI;
using Slugterra.InAppPurchases;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class InAppItemView : UIView
	{
		[SerializeField]
		private TextMesh _titleText;

		[SerializeField]
		private TextMesh _buttonText;

		[SerializeField]
		private TextMesh _moneyCountText;

		[SerializeField]
		private Badge _saleBadge;

		[SerializeField]
		private Badge _hitBadge;

		[SerializeField]
		private tk2dSprite _inAppItemSprite;

		protected override void OnStart()
		{
			base.OnStart();
			MonoSingleton<Localization>.Instance.ConfigTextMesh(_titleText);
		}

		protected override void OnApplyModel(object model)
		{
			if (model is InAppPurchase)
			{
				InAppPurchase inAppPurchase = model as InAppPurchase;
				if (!string.IsNullOrEmpty(inAppPurchase.TitleLocalizationKey))
				{
					_titleText.text = MonoSingleton<Localization>.Instance[inAppPurchase.TitleLocalizationKey];
				}
				_buttonText.text = DonDestroyMonoSingleton<SlugterraPurchaseContext>.Instance.GetFormattedPrice(inAppPurchase.ProductID);
				_moneyCountText.text = ((inAppPurchase.GoldValue > 0) ? inAppPurchase.GoldValue.ToString() : inAppPurchase.GemsValue.ToString());
				if (_inAppItemSprite != null && !string.IsNullOrEmpty(inAppPurchase.StoreSpriteName))
				{
					_inAppItemSprite.SetSprite(inAppPurchase.StoreSpriteName);
				}
				if (_saleBadge != null && inAppPurchase.Sale != 0)
				{
					_saleBadge.parent.SetActive(true);
					TextMesh label = _saleBadge.label;
					string text = string.Format("- {0} %", inAppPurchase.Sale);
					_saleBadge.shadow.text = text;
					label.text = text;
					_saleBadge.label.gameObject.SetActive(true);
					_hitBadge.label.gameObject.SetActive(false);
					_hitBadge.backGround.SetActive(false);
					_saleBadge.backGround.SetActive(true);
				}
				if (_hitBadge != null && inAppPurchase.IsBestSeller)
				{
					_hitBadge.parent.SetActive(true);
					_saleBadge.label.gameObject.SetActive(false);
					_hitBadge.label.gameObject.SetActive(true);
					_saleBadge.backGround.SetActive(false);
					_hitBadge.backGround.SetActive(true);
				}
			}
		}
	}
}

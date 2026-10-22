using AppsMinistry.Core;
using AppsMinistry.Core.Ads;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;
using Slugterra.Scene;
using Slugterra.UI.MapMenu;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class PopupOfferItemsForViewingAdsController : UIController<PopupOfferItemsForViewingAdsView>
	{
		[SerializeField]
		private GameObject _skipBtn;

		protected override void OnStart()
		{
			base.OnStart();
		}

		protected override void _OnEnable()
		{
			base._OnEnable();
			View.OnDecline += OnDecline;
			View.OnBuy += OnBuy;
		}

		protected override void _OnDisable()
		{
			base._OnDisable();
			View.OnDecline -= OnDecline;
			View.OnBuy -= OnBuy;
		}

		private void OnDecline()
		{
			SceneLoader.LoadScene<BackpackSceneController>();
		}

		private void OnBuy()
		{
			PopupConfirmView _popupConfirm = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupConfirmView>();
			DonDestroyMonoSingleton<AdsManager>.Instance.RewardSuccessEvent = TakeOffer;
			DonDestroyMonoSingleton<AdsManager>.Instance.RewardFailEvent = () =>
			{
			};
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				if (DonDestroyMonoSingleton<AdsManager>.Instance.IncentivizeAdsAvailable())
				{
					DonDestroyMonoSingleton<AdsManager>.Instance.ShowIncentivizedAds(string.Empty);
				}
				else
				{
					_popupConfirm.Show("alert_ads_not_available", () =>
					{
					}, null, false);
				}
			}, (string errorMsg) =>
			{
				_popupConfirm.Show("alert_internet_error", () =>
				{
				}, null, false);
			});
		}

		private void TakeOffer()
		{
			OffersConfiguration offersConfiguration = MonoSingleton<GameConfiguration>.Instance.OffersConfiguration;
			if (offersConfiguration == null)
			{
				return;
			}
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			if (missionSetting == null)
			{
				return;
			}
			Offer offer = offersConfiguration.GetOffer(LevelItemController.SelectedLevel.Model.Scene.Name);
			if (offer == null)
			{
				return;
			}
			if (offer != null)
			{
				if (offer.AttackSlug != SlugType.None)
				{
					missionSetting.OfferedAttackSlug = new SlugItem(offer.AttackSlug, offer.AttackSlugLevel);
				}
				if (offer.DefenceSlug != SlugType.None)
				{
					missionSetting.OfferedDefenceSlug = new SlugItem(offer.DefenceSlug, offer.DefenceSlugLevel);
				}
				if (offer.SpecialSlug != SlugType.None)
				{
					missionSetting.OfferedSpecialSlug = new SlugItem(offer.SpecialSlug, offer.SpecialSlugLevel);
				}
				if (offer.Blaster != BlasterType.None)
				{
					missionSetting.OfferedBlaster = new BlasterItem(offer.Blaster);
				}
				if (offer.Armor != ArmorType.None)
				{
					missionSetting.OfferedArmor = new ArmorItem(offer.Armor, 0, false, true);
				}
				if (offer.Assistant != AllyType.None)
				{
					missionSetting.OfferedAssistant = new AssistantItem(offer.Assistant, offer.AssistantLevel, false, true);
				}
			}
			SceneLoader.LoadScene<BackpackSceneController>();
		}
	}
}

using AppsMinistry.Core;
using AppsMinistry.Core.Ads;
using AppsMinistry.Utils;
using DG.Tweening;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Game.HealthRestoration;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class AssistantAccelerateRecoveryBarViewController : MonoBehaviour
	{
		[SerializeField]
		private TextMesh priceText;

		private AllyType _currentlySelectedAlly;

		private float _currentPrice;

		public const float MoveAnimationTime = 0.35f;

		public static readonly Vector3 HidePosition = new Vector3(0f, -1f, 0f);

		public static readonly Vector3 ShowPosition = new Vector3(0f, 0.565f, 0f);

		private Tweener _transformTweener;

		private bool _show;

		private RestoreHealthConfiguration healthRestoreManager
		{
			get
			{
				return MonoSingleton<GameConfiguration>.Instance.RestoreHealthConfigurationManager;
			}
		}

		public void SetAlly(AllyType allyType)
		{
			_currentlySelectedAlly = allyType;
		}

		private void OnEnable()
		{
			transform.localPosition = HidePosition;
			_show = false;
		}

		public void SetPrice(float price)
		{
			_currentPrice = Mathf.Round(price);
			if (priceText.text != _currentPrice.ToString())
			{
				priceText.text = _currentPrice.ToString();
			}
		}

		private void OnAdButtonClick()
		{
			DonDestroyMonoSingleton<CheckInternetService>.Instance.CheckInternet(() =>
			{
				DonDestroyMonoSingleton<AdsManager>.Instance.RewardSuccessEvent = HealAssistant;
				DonDestroyMonoSingleton<AdsManager>.Instance.RewardFailEvent = null;
				if (DonDestroyMonoSingleton<AdsManager>.Instance.IncentivizeAdsAvailable())
				{
					DonDestroyMonoSingleton<AdsManager>.Instance.ShowIncentivizedAds(string.Empty);
				}
			}, null);
		}

		private void OnMoneyButtonClick()
		{
			MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			if (missionSetting.Assistant != null)
			{
				shopModel.OnRecoverAssistant(missionSetting.Assistant);
			}
		}

		private void HealAssistant()
		{
			healthRestoreManager.GetHealthConfiguration(_currentlySelectedAlly).Heal();
			healthRestoreManager.Save();
		}

		public void Show(bool value)
		{
			if (_show == value)
			{
				return;
			}
			if (_transformTweener != null && !_transformTweener.IsComplete())
			{
				_transformTweener.Kill();
			}
			if (value)
			{
				_transformTweener = transform.DOLocalMove(ShowPosition, 0.35f).OnStart(() =>
				{
					gameObject.SetActive(true);
				});
			}
			else
			{
				_transformTweener = transform.DOLocalMove(HidePosition, 0.35f).OnComplete(() =>
				{
					gameObject.SetActive(false);
				});
			}
			_show = value;
		}

		private void Update()
		{
		}
	}
}

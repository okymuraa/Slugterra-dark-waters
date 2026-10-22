using System;
using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Ads;
using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class AdsItemView : UIView
	{
		public Action onEnableButtonEvent;

		public Action onDisableButtonEvent;

		[SerializeField]
		private GameObject _btnOn;

		[SerializeField]
		private GameObject _btnOff;

		[SerializeField]
		private GameObject _btnWatch;

		[SerializeField]
		private GameObject _noMoreAds;

		[SerializeField]
		private TextMesh _dotLbl;

		[SerializeField]
		private bool _isGemsButton;

		private Coroutine adsAvailable;

		public bool IsGemsButton
		{
			get
			{
				return _isGemsButton;
			}
		}

		private void OnEnable()
		{
			StartCoroutine("CheckAdsAvailable", 5f);
		}

		private void OnDisable()
		{
			StopAllCoroutines();
		}

		public void DisableButton()
		{
			if (_btnOn.activeSelf)
			{
				_btnOn.SetActive(false);
			}
			if (DonDestroyMonoSingleton<AdsManager>.Instance.CurrentAdsShowCounter < DonDestroyMonoSingleton<AdsManager>.Instance.RewardAdsLimitPerTimeInterval)
			{
				if (_noMoreAds.activeSelf)
				{
					_noMoreAds.SetActive(false);
				}
				if (!_btnOff.activeSelf)
				{
					_btnOff.SetActive(true);
				}
				if (!_btnWatch.activeSelf)
				{
					_btnWatch.SetActive(true);
				}
				_dotLbl.text = ".....";
			}
			else
			{
				if (_btnWatch.activeSelf)
				{
					_btnWatch.SetActive(false);
				}
				if (_btnOff.activeSelf)
				{
					_btnOff.SetActive(false);
				}
				if (!_noMoreAds.activeSelf)
				{
					_noMoreAds.SetActive(true);
				}
			}
			if (onDisableButtonEvent != null)
			{
				onDisableButtonEvent();
			}
		}

		private void EnableButton()
		{
			if (_btnOff.activeSelf)
			{
				_btnOff.SetActive(false);
			}
			if (_noMoreAds.activeSelf)
			{
				_noMoreAds.SetActive(false);
			}
			if (!_btnWatch.activeSelf)
			{
				_btnWatch.SetActive(true);
			}
			if (!_btnOn.activeSelf)
			{
				_btnOn.SetActive(true);
			}
			if (onEnableButtonEvent != null)
			{
				onEnableButtonEvent();
			}
		}

		private IEnumerator CheckAdsAvailable(float timeInterval)
		{
			float timeAccum = 999f;
			float timeAccumForDotlbl = 0f;
			int dotCounter = 0;
			while (true)
			{
				if (AppsMinistrySdkBinding.Instance.AMIsOnline())
				{
					if (_dotLbl != null && _dotLbl.gameObject.activeSelf)
					{
						timeAccumForDotlbl += Time.unscaledDeltaTime;
						if (timeAccumForDotlbl > 0.5f)
						{
							if (dotCounter == 5)
							{
								dotCounter = 0;
							}
							if (dotCounter == 5 || dotCounter == 0)
							{
								_dotLbl.text = string.Empty;
							}
							dotCounter++;
							_dotLbl.text += ".";
							timeAccumForDotlbl = 0f;
						}
					}
					if (!IsGemsButton && DonDestroyMonoSingleton<AdsManager>.Instance.CurrentAdsShowCounter >= DonDestroyMonoSingleton<AdsManager>.Instance.RewardAdsLimitPerTimeInterval)
					{
						DisableButton();
					}
					timeAccum += Time.unscaledDeltaTime;
					if (timeAccum > timeInterval)
					{
						timeAccum = 0f;
						if (DonDestroyMonoSingleton<AdsManager>.Instance.RewardIncentivizeAdsAvailable)
						{
							EnableButton();
						}
						else
						{
							DisableButton();
						}
					}
				}
				yield return null;
			}
		}
	}
}

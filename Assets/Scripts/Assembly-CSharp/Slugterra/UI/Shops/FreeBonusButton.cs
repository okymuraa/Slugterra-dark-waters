using System;
using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Ads;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class FreeBonusButton : MonoBehaviour
	{
		public Action onEnableButtonEvent;

		public Action onDisableButtonEvent;

		[SerializeField]
		private GameObject _btnOn;

		[SerializeField]
		private GameObject _btnOff;

		[SerializeField]
		private tk2dSlicedSprite[] _slicedSprites;

		[SerializeField]
		private TextMesh _dotLbl;

		private Coroutine adsAvailable;

		private void OnEnable()
		{
			adsAvailable = StartCoroutine(CheckAdsAvailable(5f));
		}

		private void OnDisable()
		{
			if (adsAvailable != null)
			{
				StopCoroutine(adsAvailable);
			}
		}

		public void DisableButton()
		{
			if (_btnOn != null)
			{
				_btnOn.SetActive(false);
			}
			if (_btnOff != null)
			{
				_btnOff.SetActive(true);
			}
			for (int i = 0; i < _slicedSprites.Length; i++)
			{
				if (_slicedSprites[i] != null)
				{
					_slicedSprites[i].gameObject.SetActive(false);
				}
			}
			if (DonDestroyMonoSingleton<AdsManager>.Instance.CurrentAdsShowCounter < DonDestroyMonoSingleton<AdsManager>.Instance.RewardAdsLimitPerTimeInterval)
			{
				_dotLbl.gameObject.SetActive(true);
			}
			else
			{
				_dotLbl.gameObject.SetActive(false);
			}
			if (onDisableButtonEvent != null)
			{
				onDisableButtonEvent();
			}
		}

		private void EnableButton()
		{
			if (_btnOff != null)
			{
				_btnOff.SetActive(false);
			}
			if (_btnOn != null)
			{
				_btnOn.SetActive(true);
			}
			for (int i = 0; i < _slicedSprites.Length; i++)
			{
				if (_slicedSprites[i] != null)
				{
					_slicedSprites[i].gameObject.SetActive(true);
				}
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
					if (DonDestroyMonoSingleton<AdsManager>.Instance.CurrentAdsShowCounter >= DonDestroyMonoSingleton<AdsManager>.Instance.RewardAdsLimitPerTimeInterval)
					{
						DisableButton();
					}
					timeAccum += Time.unscaledDeltaTime;
					if (timeAccum > timeInterval)
					{
						timeAccum = 0f;
						if (DonDestroyMonoSingleton<AdsManager>.Instance.IncentivizeAdsAvailable())
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

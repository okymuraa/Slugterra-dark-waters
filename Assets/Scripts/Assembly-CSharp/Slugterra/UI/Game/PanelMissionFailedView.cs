using System.Collections;
using AppsMinistry.Core;
using AppsMinistry.Core.Ads;
using AppsMinistry.UI;
using AppsMinistry.Utils;
using UnityEngine;

namespace Slugterra.UI.Game
{
	public class PanelMissionFailedView : UIView
	{
		[SerializeField]
		private tk2dSlicedSprite[] _slicedSprites;

		[SerializeField]
		private tk2dSprite _btnIcon;

		[SerializeField]
		private tk2dButton _respawnBtn;

		[SerializeField]
		private TextMesh respawnCounter;

		[SerializeField]
		private TextMesh _dotLbl;

		[SerializeField]
		private GameObject _warningMessage;

		[SerializeField]
		private GameObject _freeRespawn;

		private int _useAdsRespawnCount;

		private Coroutine adsAvailable;

		public GameObject WarningMessage
		{
			get
			{
				return _warningMessage;
			}
		}

		public GameObject FreeRespawn
		{
			get
			{
				return _freeRespawn;
			}
		}

		public int UseAdsRespawnCount
		{
			get
			{
				return _useAdsRespawnCount;
			}
			set
			{
				_useAdsRespawnCount = value;
			}
		}

		private void OnEnable()
		{
			DisableButton();
			adsAvailable = StartCoroutine(CheckAdsAvailable(2f));
		}

		private void OnDisable()
		{
			if (adsAvailable != null)
			{
				StopCoroutine(adsAvailable);
			}
		}

		private void DisableButton()
		{
			for (int i = 0; i < _slicedSprites.Length; i++)
			{
				_slicedSprites[i].color = new Color(_slicedSprites[i].color.r, _slicedSprites[i].color.g, _slicedSprites[i].color.b, 1f / 3f);
			}
			BoxCollider component = _respawnBtn.gameObject.GetComponent<BoxCollider>();
			component.enabled = false;
			_dotLbl.gameObject.SetActive(true);
			_btnIcon.gameObject.SetActive(false);
			respawnCounter.gameObject.SetActive(false);
		}

		private void EnableButton()
		{
			for (int i = 0; i < _slicedSprites.Length; i++)
			{
				_slicedSprites[i].color = new Color(_slicedSprites[i].color.r, _slicedSprites[i].color.g, _slicedSprites[i].color.b, 1f);
			}
			BoxCollider component = _respawnBtn.gameObject.GetComponent<BoxCollider>();
			component.enabled = true;
			_btnIcon.gameObject.SetActive(true);
			respawnCounter.gameObject.SetActive(true);
		}

		private IEnumerator CheckAdsAvailable(float timeInterval)
		{
			float timeAccum = 999f;
			float timeAccumForDotlbl = 0f;
			int dotCounter = 0;
			while (true)
			{
				bool internetStatus = DonDestroyMonoSingleton<CheckInternetService>.Instance.IsInternetAvailable;
				int respawnAdsCount = Mathf.Clamp(DonDestroyMonoSingleton<AdsManager>.Instance.RespawnForAdsLimitPerLevel - _useAdsRespawnCount, 0, DonDestroyMonoSingleton<AdsManager>.Instance.RespawnForAdsLimitPerLevel);
				if (_useAdsRespawnCount >= DonDestroyMonoSingleton<AdsManager>.Instance.RespawnForAdsLimitPerLevel)
				{
					respawnCounter.text = "0";
				}
				else
				{
					respawnAdsCount = Mathf.Clamp(respawnAdsCount, 0, DonDestroyMonoSingleton<AdsManager>.Instance.RespawnForAdsLimitPerLevel);
					respawnCounter.text = respawnAdsCount.ToString();
				}
				if (internetStatus)
				{
					if (_dotLbl.gameObject.activeSelf)
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
					if (_useAdsRespawnCount >= DonDestroyMonoSingleton<AdsManager>.Instance.RespawnForAdsLimitPerLevel || respawnAdsCount == 0)
					{
						FreeRespawn.SetActive(false);
						WarningMessage.SetActive(true);
					}
					else
					{
						timeAccum += Time.unscaledDeltaTime;
						if (timeAccum > timeInterval)
						{
							timeAccum = 0f;
							if (respawnAdsCount <= DonDestroyMonoSingleton<AdsManager>.Instance.RespawnForAdsLimitPerLevel)
							{
								if (!DonDestroyMonoSingleton<AdsManager>.Instance.IncentivizeAdsAvailable())
								{
									DisableButton();
								}
								else
								{
									_dotLbl.gameObject.SetActive(false);
									EnableButton();
								}
							}
						}
					}
				}
				else
				{
					FreeRespawn.SetActive(false);
					WarningMessage.SetActive(true);
				}
				yield return null;
			}
		}
	}
}

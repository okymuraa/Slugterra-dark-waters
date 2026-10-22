using UnityEngine;

namespace Slugterra.UI.Shops.PaymentStates
{
	public abstract class AbstractPaymentState : MonoBehaviour
	{
		[SerializeField]
		private GameObject _rootObject;

		[SerializeField]
		protected TextMesh _title;

		[SerializeField]
		protected GameObject _coinsButton;

		[SerializeField]
		protected GameObject _moneyButton;

		[SerializeField]
		protected GameObject _gemsButton;

		[SerializeField]
		protected GameObject _adsButton;

		[SerializeField]
		protected TextMesh _priceCoins;

		[SerializeField]
		protected TextMesh _priceMoney;

		[SerializeField]
		protected TextMesh _priceGems;

		public virtual void Show()
		{
			if (_rootObject != null)
			{
				if (!_rootObject.activeSelf)
				{
					_rootObject.SetActive(true);
				}
			}
			else
			{
				Debug.Log("<color=red>Link to _rootObject of AbstractPaymentState is null, FIX IT</Color>");
			}
		}

		public virtual void Hide()
		{
			if (_rootObject != null)
			{
				if (_rootObject.activeSelf)
				{
					_rootObject.SetActive(false);
				}
			}
			else
			{
				Debug.Log("<color=red>Link to _rootObject of AbstractPaymentState is null, FIX IT</Color>");
			}
		}

		public virtual void SetPrice(PaymentButtonCurrency currency, string price)
		{
			switch (currency)
			{
			case PaymentButtonCurrency.Coins:
				if (_moneyButton != null && _moneyButton.activeSelf)
				{
					_moneyButton.SetActive(false);
				}
				if (_gemsButton != null && _gemsButton.activeSelf)
				{
					_gemsButton.SetActive(false);
				}
				if (_adsButton != null && _adsButton.activeSelf)
				{
					_adsButton.SetActive(false);
				}
				if (_coinsButton != null)
				{
					if (!_coinsButton.activeSelf)
					{
						_coinsButton.SetActive(true);
					}
				}
				else
				{
					Debug.Log("<color=red>Link to _coinsButton of AbstractPaymentState is null, FIX IT</Color>");
				}
				_priceCoins.text = price;
				break;
			case PaymentButtonCurrency.Money:
				if (_coinsButton != null && _coinsButton.activeSelf)
				{
					_coinsButton.SetActive(false);
				}
				if (_gemsButton != null && _gemsButton.activeSelf)
				{
					_gemsButton.SetActive(false);
				}
				if (_adsButton != null && _adsButton.activeSelf)
				{
					_adsButton.SetActive(false);
				}
				if (_moneyButton != null)
				{
					if (!_moneyButton.activeSelf)
					{
						_moneyButton.SetActive(true);
					}
				}
				else
				{
					Debug.Log("<color=red>Link to _moneyButton of AbstractPaymentState is null, FIX IT</Color>");
				}
				_priceMoney.text = price;
				break;
			case PaymentButtonCurrency.Gems:
				if (_coinsButton != null && _coinsButton.activeSelf)
				{
					_coinsButton.SetActive(false);
				}
				if (_moneyButton != null && _moneyButton.activeSelf)
				{
					_moneyButton.SetActive(false);
				}
				if (_adsButton != null && _adsButton.activeSelf)
				{
					_adsButton.SetActive(false);
				}
				if (_gemsButton != null)
				{
					if (!_gemsButton.activeSelf)
					{
						_gemsButton.SetActive(true);
					}
				}
				else
				{
					Debug.Log("<color=red>Link to _gemsButton of AbstractPaymentState is null, FIX IT</Color>");
				}
				_priceGems.text = price;
				break;
			case PaymentButtonCurrency.Ads:
				if (_coinsButton != null && _coinsButton.activeSelf)
				{
					_coinsButton.SetActive(false);
				}
				if (_moneyButton != null && _moneyButton.activeSelf)
				{
					_moneyButton.SetActive(false);
				}
				if (_gemsButton != null && _gemsButton.activeSelf)
				{
					_gemsButton.SetActive(false);
				}
				if (_adsButton != null)
				{
					if (!_adsButton.activeSelf)
					{
						_adsButton.SetActive(true);
					}
				}
				else
				{
					Debug.Log("<color=red>Link to _gemsButton of AbstractPaymentState is null, FIX IT</Color>");
				}
				break;
			}
		}

		public void ResetButtonsScale()
		{
			if (_coinsButton != null)
			{
				_coinsButton.GetComponent<tk2dButton>().ResetScale();
			}
			if (_moneyButton != null)
			{
				_moneyButton.GetComponent<tk2dButton>().ResetScale();
			}
			if (_gemsButton != null)
			{
				_gemsButton.GetComponent<tk2dButton>().ResetScale();
			}
			if (_adsButton != null)
			{
				_adsButton.GetComponent<tk2dButton>().ResetScale();
			}
		}
	}
}

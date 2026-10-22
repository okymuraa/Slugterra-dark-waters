using UnityEngine;

namespace Slugterra.UI.Shops.PaymentStates
{
	public class UpgradePaymentState : AbstractPaymentState
	{
		[SerializeField]
		private GameObject _maxUpgradeRoot;

		[SerializeField]
		private GameObject _upgradeRoot;

		[SerializeField]
		private GameObject _lockedRoot;

		public bool IsMax;

		public void SetUpgrade(bool isMax)
		{
			IsMax = isMax;
			if (_maxUpgradeRoot != null)
			{
				_maxUpgradeRoot.SetActive(isMax);
			}
			_upgradeRoot.SetActive(!isMax);
			BoxCollider component = GetComponent<BoxCollider>();
			if (component != null)
			{
				component.enabled = !isMax;
			}
			if (_lockedRoot != null)
			{
				_lockedRoot.SetActive(false);
			}
		}

		public void SetLocked()
		{
			_upgradeRoot.SetActive(false);
			if (_maxUpgradeRoot != null)
			{
				_maxUpgradeRoot.SetActive(false);
			}
			if (_lockedRoot != null)
			{
				_lockedRoot.SetActive(true);
			}
		}

		public void SetLockedText(string text)
		{
			if (_lockedRoot != null)
			{
				_lockedRoot.GetComponentInChildren<TextMesh>().text = text;
			}
		}
	}
}

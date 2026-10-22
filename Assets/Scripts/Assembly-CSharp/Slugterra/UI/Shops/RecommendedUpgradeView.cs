using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.UI.Popup;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class RecommendedUpgradeView : UIView
	{
		[SerializeField]
		private PaymentButtonController _paymentBtn;

		[SerializeField]
		private AlphaPulseAnimUI[] _recLevels;

		[SerializeField]
		private GameObject _recommendationBadge;

		[SerializeField]
		private GameObject _requiredBadge;

		[SerializeField]
		private TextMesh _recLbl;

		[SerializeField]
		private TextMesh _recLblOutline;

		protected override void OnApplyModel(object model)
		{
			RecommendationDataContainer recommendedUpgrade = (RecommendationDataContainer)model;
			SetRecommendedUpgrade(recommendedUpgrade);
		}

		private void SetRecommendedUpgrade(RecommendationDataContainer data)
		{
			for (int i = 0; i < _recLevels.Length; i++)
			{
				if (i >= data.ItemLevel && i < data.RecommendedLevel)
				{
					_recLevels[i].Sprite.gameObject.SetActive(true);
					_recLevels[i].LaunchAnimation();
				}
				else if (i >= data.ItemLevel && i >= data.RecommendedLevel)
				{
					_recLevels[i].Sprite.gameObject.SetActive(false);
					_recLevels[i].LaunchAnimation(false);
				}
				else
				{
					_recLevels[i].Sprite.gameObject.SetActive(true);
					_recLevels[i].LaunchAnimation(false);
				}
			}
			_recommendationBadge.SetActive(data.ItemLevel < data.RecommendedLevel);
			if (_requiredBadge != null)
			{
				_requiredBadge.SetActive(data.IsRequired);
			}
			if (data.ItemLevel < data.RecommendedLevel)
			{
				TextMesh recLbl = _recLbl;
				string value = Localization.GetValue("lvl_0" + data.RecommendedLevel + "_title" + ((data.RecommendedLevel != 5) ? string.Empty : "_short"), string.Empty);
				_recLblOutline.text = value;
				recLbl.text = value;
			}
		}

		public void Deactivate()
		{
			for (int i = 0; i < _recLevels.Length; i++)
			{
				_recLevels[i].Sprite.gameObject.SetActive(false);
				_recLevels[i].LaunchAnimation(false);
			}
			_recommendationBadge.SetActive(false);
		}
	}
}

using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Multiplayer
{
	public class RewardItemView : UIView
	{
		[SerializeField]
		private TextMesh _rank;

		[SerializeField]
		private TextMesh _speedBoostCount;

		[SerializeField]
		private tk2dSprite _speedBoostIcon;

		[SerializeField]
		private TextMesh _slugFoodBoostCount;

		[SerializeField]
		private tk2dSprite _slugFoodBoostIcon;

		[SerializeField]
		private TextMesh _armorBoostCount;

		[SerializeField]
		private tk2dSprite _armorBoostIcon;

		[SerializeField]
		private TextMesh _healthBoostCount;

		[SerializeField]
		private tk2dSprite _healthBoostIcon;

		[SerializeField]
		private TextMesh _starsCount;

		[SerializeField]
		private TextMesh _coinsCount;
	}
}

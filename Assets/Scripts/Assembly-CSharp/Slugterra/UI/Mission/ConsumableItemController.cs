using AppsMinistry.UI;

namespace Slugterra.UI.Mission
{
	public class ConsumableItemController : UIController<ConsumableItemView>
	{
		protected override void _OnEnable()
		{
			View.OnBuyEvent += OnBuy;
		}

		protected override void _OnDisable()
		{
			View.OnBuyEvent -= OnBuy;
		}

		private void OnBuy()
		{
		}
	}
}

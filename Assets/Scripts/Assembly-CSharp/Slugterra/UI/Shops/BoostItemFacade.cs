using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class BoostItemFacade : ItemFacade
	{
		[SerializeField]
		private GameObject _boostBar;

		private ConsumableItem _model;

		protected override void OnEnable()
		{
			base.OnEnable();
			_boostBar.SetActive(true);
		}

		protected override void OnDisable()
		{
			_boostBar.SetActive(false);
			base.OnDisable();
		}

		protected override void OnApplyModel(object model)
		{
			if (model is ConsumableItem)
			{
				_model = model as ConsumableItem;
			}
		}
	}
}

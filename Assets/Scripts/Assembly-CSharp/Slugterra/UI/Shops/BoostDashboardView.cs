using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class BoostDashboardView : ItemDashboardView
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Action<int> OnBuyEvent__BackingField;

		[SerializeField]
		private TextMesh _smallPackPrice;

		[SerializeField]
		private TextMesh _largePackPrice;

		public event Action<int> OnBuyEvent
		{
			add
			{
				Action<int> action = OnBuyEvent__BackingField;
				Action<int> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBuyEvent__BackingField, (Action<int>)Delegate.Combine(action2, value), action);
				}
				while ((object)action != action2);
			}
			remove
			{
				Action<int> action = OnBuyEvent__BackingField;
				Action<int> action2;
				do
				{
					action2 = action;
					action = Interlocked.CompareExchange(ref OnBuyEvent__BackingField, (Action<int>)Delegate.Remove(action2, value), action);
				}
				while ((object)action != action2);
			}
		}

		private void OnBuySmallPackClick()
		{
			if (OnBuyEvent__BackingField != null)
			{
				OnBuyEvent__BackingField(5);
			}
		}

		private void OnBuyLargePackClick()
		{
			if (OnBuyEvent__BackingField != null)
			{
				OnBuyEvent__BackingField(10);
			}
		}

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (model is ConsumableItem)
			{
				Localize(model);
				ConsumableItem consumableItem = model as ConsumableItem;
				GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
				ConsumableProperties consumableProperties = instance.ConsumableConfigurationManager.GetConsumableProperties(consumableItem.Type);
				_smallPackPrice.text = consumableProperties.priceData.smallPackPrice.ToString();
				_largePackPrice.text = consumableProperties.priceData.largePackPrice.ToString();
			}
		}

		private void Localize(object model)
		{
			if (model is ConsumableItem)
			{
				ConsumableItem consumableItem = model as ConsumableItem;
				GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
				ConsumableProperties consumableProperties = instance.ConsumableConfigurationManager.GetConsumableProperties(consumableItem.Type);
				_title.text = MonoSingleton<Localization>.Instance.GetValueByKey(consumableProperties.baseData.title);
				_description.text = MonoSingleton<Localization>.Instance.GetValueByKey(consumableProperties.baseData.description);
			}
		}
	}
}

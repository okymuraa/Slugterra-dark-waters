using System;
using System.Diagnostics;
using System.Threading;
using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using UnityEngine;

public class GameConsumableView : UIView
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private Action<int> OnBuyEvent__BackingField;

	[SerializeField]
	private tk2dSprite _itemSprite;

	[SerializeField]
	private tk2dBaseSprite _frame;

	[SerializeField]
	private TextMesh _countText;

	[SerializeField]
	private TextMesh _titleText;

	[SerializeField]
	private TextMesh _descriptionText;

	[SerializeField]
	private TextMesh _largePackPriceText;

	[SerializeField]
	private TextMesh _smallPackPriceText;

	[SerializeField]
	private tk2dButton _smallPackButtonBtnBuy;

	[SerializeField]
	private tk2dButton _bigPackButtonBtnBuy;

	public tk2dButton SmallPackButtonBtnBuy
	{
		get
		{
			return _smallPackButtonBtnBuy;
		}
	}

	public tk2dButton BigPackButtonBtnBuy
	{
		get
		{
			return _bigPackButtonBtnBuy;
		}
	}

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

	protected override void OnStart()
	{
		base.OnStart();
		MonoSingleton<Localization>.Instance.ConfigTextMesh(_titleText);
		MonoSingleton<Localization>.Instance.ConfigTextMesh(_descriptionText);
	}

	private void OnBtnSmallPackClick()
	{
		if (OnBuyEvent__BackingField != null)
		{
			OnBuyEvent__BackingField(5);
		}
	}

	private void OnBtnLargePackClick()
	{
		if (OnBuyEvent__BackingField != null)
		{
			OnBuyEvent__BackingField(10);
		}
	}

	protected override void OnApplyModel(object model)
	{
		if (model is ConsumableItem)
		{
			ConsumableItem consumableItem = model as ConsumableItem;
			_countText.text = consumableItem.Count.ToString();
			Localize(model);
			string sprite = string.Empty;
			string sprite2 = string.Empty;
			Color color = Color.white;
			if (consumableItem.Type == ConsumableType.First)
			{
				sprite = "icon_health";
				sprite2 = "frame_green";
				color = new Color32(0, 226, 50, byte.MaxValue);
			}
			if (consumableItem.Type == ConsumableType.ArmorBoost)
			{
				sprite = "icon_armor_normal";
				sprite2 = "frame_blue";
				color = new Color32(0, 207, 236, byte.MaxValue);
			}
			if (consumableItem.Type == ConsumableType.SpeedBoost)
			{
				sprite = "icon_speed";
				sprite2 = "frame_indigo";
				color = new Color32(0, 185, byte.MaxValue, byte.MaxValue);
			}
			if (consumableItem.Type == ConsumableType.SlugFood)
			{
				sprite = "icon_slugs_selected";
				sprite2 = "frame_orange";
				color = new Color32(byte.MaxValue, 179, 0, byte.MaxValue);
			}
			_itemSprite.SetSprite(sprite);
			_frame.SetSprite(sprite2);
			_countText.color = color;
		}
	}

	private void Localize(object model)
	{
		if (model is ConsumableItem)
		{
			ConsumableItem consumableItem = model as ConsumableItem;
			GameConfiguration instance = MonoSingleton<GameConfiguration>.Instance;
			ConsumableProperties consumableProperties = instance.ConsumableConfigurationManager.GetConsumableProperties(consumableItem.Type);
			_largePackPriceText.text = consumableProperties.priceData.largePackPrice.ToString();
			_smallPackPriceText.text = consumableProperties.priceData.smallPackPrice.ToString();
			_titleText.text = MonoSingleton<Localization>.Instance["ingame_" + consumableProperties.baseData.title];
			_descriptionText.text = MonoSingleton<Localization>.Instance["ingame_" + consumableProperties.baseData.description];
		}
	}
}

using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.Domain.Shops;
using Slugterra.UI.Utils;
using UnityEngine;

namespace Slugterra.UI.Popup
{
	public class OfferedItemView : UIView
	{
		[SerializeField]
		private tk2dSprite _slug;

		[SerializeField]
		private tk2dSprite _blaster;

		[SerializeField]
		private TextMesh _forceCount;

		protected override void OnApplyModel(object model)
		{
			base.OnApplyModel(model);
			if (model is BlasterItem)
			{
				_blaster.gameObject.SetActive(true);
				_slug.gameObject.SetActive(false);
			}
			else
			{
				_blaster.gameObject.SetActive(false);
				_slug.gameObject.SetActive(true);
			}
			if (model is SlugItem)
			{
				SlugItem slugItem = model as SlugItem;
				string sprite = SpriteNamesHelper.GetSlugSpriteName(slugItem.Type).Replace("icon", "item");
				_slug.SetSprite(sprite);
				SlugConfig slugConfiguration = MonoSingleton<GameConfiguration>.Instance.SlugConfigManager.GetSlugConfiguration(slugItem.Type);
				_forceCount.text = slugConfiguration.levels[slugItem.Level].forceRate.ToString();
			}
			else if (model is BlasterItem)
			{
				BlasterItem blasterItem = model as BlasterItem;
				BlasterConfig blusterConfiguration = MonoSingleton<GameConfiguration>.Instance.BlasterConfigManager.GetBlusterConfiguration(blasterItem.Type);
				_blaster.SetSprite(blusterConfiguration.uiIcon);
				_forceCount.text = blusterConfiguration.levels[blasterItem.Level].forceRate.ToString();
			}
			else if (model is ArmorItem)
			{
				ArmorItem armorItem = model as ArmorItem;
				ArmorConfig armorConfiguration = MonoSingleton<GameConfiguration>.Instance.ArmorConfigManager.GetArmorConfiguration(armorItem.Type);
				_slug.SetSprite(armorConfiguration.uiIcon);
				_forceCount.text = armorConfiguration.levels[armorItem.Level].forceRate.ToString();
			}
			else if (model is AssistantItem)
			{
				AssistantItem assistantItem = model as AssistantItem;
			}
		}
	}
}

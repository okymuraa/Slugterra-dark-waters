using AppsMinistry.UI;
using Slugterra.Configuration;
using Slugterra.UI.Popup;

namespace Slugterra.UI.Shops
{
	public class SpecialPackItemElementController : UIController<SpecialPackItemElementView>
	{
		private ItemType _modelType;

		private PopupGuideAnnouncementView _popupContainer;

		private object _modelValue;

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		public void ShowArmor(ArmorType type, AnnounceItemViewType viewType)
		{
			_modelType = ItemType.Armor;
			_modelValue = type;
			View.ShowArmor(type);
		}

		public void ShowBlaster(BlasterType type, AnnounceItemViewType viewType, PopupGuideAnnouncementView popup)
		{
			_modelType = ItemType.Blaster;
			_modelValue = type;
			View.ShowBlaster(type);
		}

		public void ShowSlug(SlugType type, AnnounceItemViewType viewType, PopupGuideAnnouncementView popup, int upgradeLevel)
		{
			_modelType = ItemType.Slug;
			_modelValue = type;
			View.ShowSlug(type, upgradeLevel);
		}

		public void ShowAlly(AllyType type, AnnounceItemViewType viewType, PopupGuideAnnouncementView popup, int upgradeLevel)
		{
			_modelType = ItemType.Ally;
			_modelValue = type;
			View.ShowAlly(type, upgradeLevel);
		}

		private void Update()
		{
		}
	}
}

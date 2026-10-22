using System;
using AppsMinistry.Core;
using Slugterra.App;
using Slugterra.Configuration;
using Slugterra.Domain.App.Settings;
using Slugterra.Domain.Shops;

namespace Slugterra.UI.Popup
{
	public class PopupOfferAssistantAvailabilityView : PopupGuideAnnouncementView
	{
		private AssistantItem _assistant;

		public override void Open(AnnouncementContents content, Action<AnnounceStatus> onClose, bool isBadge = false)
		{
			gameObject.SetActive(true);
			_content = content;
			_onClose = onClose;
			_titleText.text = Localization.GetValue(content.TitleTextKey, string.Empty);
			_offerText.text = Localization.GetValue(content.ContentTextKey, string.Empty);
			AllyType assistantType = content.ListContents.Allies[0];
			HumanoidConfig alliesConfig = MonoSingleton<GameConfiguration>.Instance.UnitConfigurationManager.GetAlliesConfig(assistantType);
			_assistant = MonoSingleton<AppController>.Instance.Model.ShopModel.Assistants.Find((AssistantItem assistant) => assistant.Type == assistantType);
			ShopItemConfiguration assistantConfig = MonoSingleton<AppConfiguration>.Instance.ShopConfig.GetAssistantConfig(_assistant.Type, 0);
			_buyPrice.text = assistantConfig.GamePrice.Value.ToString();
			switch (assistantType)
			{
			case AllyType.Pronto:
				_prontoIcon.SetActive(true);
				break;
			case AllyType.Trixie:
				_trixieIcon.SetActive(true);
				break;
			case AllyType.Kord:
				_kordIcon.SetActive(true);
				break;
			}
		}

		protected override void OnBtnBuyClick()
		{
			ShopModel shopModel = MonoSingleton<AppController>.Instance.Model.ShopModel;
			if (shopModel.OnBuyAssistant(_assistant))
			{
				MissionSetting missionSetting = MonoSingleton<AppController>.Instance.Model.MissionSetting;
				if (missionSetting.Assistant.Type != _assistant.Type)
				{
					missionSetting.Assistant = _assistant;
				}
				_onClose(AnnounceStatus.Done);
				OnClose();
			}
		}
	}
}

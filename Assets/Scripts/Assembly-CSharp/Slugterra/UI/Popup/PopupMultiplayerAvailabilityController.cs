using AppsMinistry.UI;
using AppsMinistry.Utils;
using Slugterra.Scene;
using Slugterra.Scene.Game;

namespace Slugterra.UI.Popup
{
	public class PopupMultiplayerAvailabilityController : UIController<PopupMultiplayerAvailabilityView>
	{
		protected override void _OnEnable()
		{
			View.OnCampaignEvent += OnCampaignBtn;
			View.OnMultiplayerEvent += OnMultiplayer;
		}

		protected override void _OnDisable()
		{
			View.OnCampaignEvent -= OnCampaignBtn;
			View.OnMultiplayerEvent -= OnMultiplayer;
		}

		private void OnCampaignBtn()
		{
			if (PlayerPrefsSystem.HasKey(typeof(ShaneTutor_0_1).Name))
			{
				SceneLoader.LoadScene<MapSceneController>();
			}
			else
			{
				SceneLoader.LoadScene<ShaneTutor_0_1>();
			}
		}

		private void OnMultiplayer()
		{
			SceneLoader.LoadScene<MultiplayerSceneController>();
		}
	}
}

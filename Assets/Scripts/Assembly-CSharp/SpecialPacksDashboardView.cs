using AppsMinistry.Core;
using AppsMinistry.UI;
using Slugterra.UI.Popup;
using Slugterra.UI.Shops;

public class SpecialPacksDashboardView : ItemDashboardView
{
	private void OpenSpecialPacksPopup()
	{
		PopupSpecialPacksView registeredPopup = DonDestroyMonoSingleton<PopupManager>.Instance.GetRegisteredPopup<PopupSpecialPacksView>();
		if (registeredPopup != null)
		{
			registeredPopup.Show();
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}

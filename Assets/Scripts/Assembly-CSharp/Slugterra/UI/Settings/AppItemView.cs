using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI.Settings
{
	public class AppItemView : UIView
	{
		[SerializeField]
		private string _applicationiOSUrl = string.Empty;

		[SerializeField]
		private string _applicationAndroidUrl = string.Empty;

		private void OnItemClick()
		{
			Application.OpenURL(_applicationAndroidUrl);
		}
	}
}

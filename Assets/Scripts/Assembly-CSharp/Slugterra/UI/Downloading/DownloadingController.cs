using AppsMinistry.Core;
using AppsMinistry.UI;
using AppsMinistry.Utils;

namespace Slugterra.UI.Downloading
{
	public class DownloadingController : UIController<DownloadingView>
	{
		protected override void OnStart()
		{
		}

		protected override void _OnEnable()
		{
			DonDestroyMonoSingleton<DownloadManager>.Instance.DownloadingController = this;
		}

		protected override void _OnDisable()
		{
		}

		protected override void OnReleaseResource()
		{
			base.OnReleaseResource();
			if (DonDestroyMonoSingleton<DownloadManager>.IsAlive)
			{
				DonDestroyMonoSingleton<DownloadManager>.Instance.DownloadingController = null;
			}
		}
	}
}

using AppsMinistry.Core;

namespace AppsMinistry
{
	public class GoogleTagManager : DonDestroyMonoSingleton<GoogleTagManager>
	{
		private ITagManager _strategy;

		private void Awake()
		{
			_strategy = new TagManager_Android();
		}
	}
}

using UnityEngine;

namespace AppsMinistry.Core
{
	[DisallowMultipleComponent]
	public class LocalizationLogo : MonoBehaviour
	{
		protected tk2dSprite _logo;

		[SerializeField]
		private string _logoName = "slugterra_logo";

		private void Start()
		{
			_logo = GetComponent<tk2dSprite>();
			ChangeLanguageCommand();
		}

		public void ChangeLanguageCommand()
		{
			if (_logo == null)
			{
				_logo = GetComponent<tk2dSprite>();
			}
			if (_logo != null)
			{
				string logoName = _logoName;
				_logo.SetSprite(MonoSingleton<Localization>.Instance.GetValueByKey(logoName, "logo_slugterra_darkwaters"));
			}
		}
	}
}

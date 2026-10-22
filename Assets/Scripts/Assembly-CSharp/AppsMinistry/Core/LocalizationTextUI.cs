using UnityEngine;
using UnityEngine.UI;

namespace AppsMinistry.Core
{
	[DisallowMultipleComponent]
	public class LocalizationTextUI : MonoBehaviour
	{
		protected Text _textUgui;

		private string _localizationKey = string.Empty;

		private void Start()
		{
			ChangeLanguageCommand();
		}

		protected virtual string GetKey()
		{
			return (!(_textUgui != null)) ? string.Empty : _textUgui.text;
		}

		public void ChangeLanguageCommand()
		{
			if (_textUgui == null)
			{
				_textUgui = GetComponent<Text>();
			}
			if (_textUgui != null)
			{
				if (string.IsNullOrEmpty(_localizationKey))
				{
					_localizationKey = GetKey();
				}
				_textUgui.text = MonoSingleton<Localization>.Instance.GetValueByKey(_localizationKey, _localizationKey);
			}
		}
	}
}

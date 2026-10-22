using UnityEngine;

namespace AppsMinistry.Core
{
	[DisallowMultipleComponent]
	public class LocalizationTextMesh : MonoBehaviour
	{
		protected TextMesh _textMesh;

		private string _localizationKey = string.Empty;

		private void Start()
		{
			ChangeLanguageCommand();
		}

		protected virtual string GetKey()
		{
			return (!(_textMesh != null)) ? string.Empty : _textMesh.text;
		}

		public void ChangeLanguageCommand()
		{
			if (_textMesh == null)
			{
				_textMesh = GetComponent<TextMesh>();
			}
			if (_textMesh != null)
			{
				if (string.IsNullOrEmpty(_localizationKey))
				{
					_localizationKey = GetKey();
				}
				MonoSingleton<Localization>.Instance.LocalizeTextMesh(_textMesh, _localizationKey, _localizationKey);
			}
		}
	}
}

using System;

namespace AppsMinistry.Scene
{
	public class SurvivalDescriptionAttribute : Attribute
	{
		private string _titleKey;

		private string _descKey;

		private string _previewKey;

		public string TitleKey
		{
			get
			{
				return _titleKey;
			}
		}

		public string DescKey
		{
			get
			{
				return _descKey;
			}
		}

		public string PreviewKey
		{
			get
			{
				return _previewKey;
			}
		}

		public SurvivalDescriptionAttribute(string titleKey, string descKey, string previewKey)
		{
			_titleKey = titleKey;
			_descKey = descKey;
			_previewKey = previewKey;
		}
	}
}

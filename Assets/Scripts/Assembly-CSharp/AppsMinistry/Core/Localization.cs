using System;
using System.Collections.Generic;
using ArabicSupport;
using Newtonsoft.Json;
using UnityEngine;

namespace AppsMinistry.Core
{
	[AppSingleton]
	public class Localization : MonoSingleton<Localization>
	{
		public static readonly string ArabicLocale = "ar";

		public static readonly string EnglishLocale = "en";

		public static readonly string EnglishLanguage = "English";

		private LocalizeLanguages localizations;

		private KeyValueLocalize currentLocalization;

		private ResourceLoader<Font> fonts = new ResourceLoader<Font>();

		private ResourceLoader<Material> fontMaterials = new ResourceLoader<Material>();

		private string _locale = EnglishLocale;

		private string _language = EnglishLanguage;

		private bool _arabic;

		public string Language
		{
			get
			{
				return _language;
			}
		}

		public string Locale
		{
			get
			{
				return _locale;
			}
		}

		public string this[string key]
		{
			get
			{
				return GetValueByKey(key);
			}
		}

		protected override void OnCreate()
		{
			Load();
			ChangeLanguage(true);
			Debug.Log("Localization::OnCreate currentLanguage " + _language);
		}

		public void ChangeLanguage(bool isLaunch = false)
		{
			if (!PlayerPrefs.HasKey("selectedLanguage"))
			{
				_locale = GetCurrentLocale();
			}
			else
			{
				_locale = PlayerPrefs.GetString("selectedLanguage");
			}
			if (!isLaunch)
			{
				localizations.localization.Remove("Arabic");
				string[] array = new string[localizations.localization.Keys.Count];
				localizations.localization.Keys.CopyTo(array, 0);
				int num = 0;
				if (localizations.locales.ContainsKey(_locale))
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].ToLower() == localizations.locales[_locale].ToLower())
						{
							num = i;
							break;
						}
					}
				}
				num++;
				num %= array.Length;
				string text = array[num];
				foreach (KeyValuePair<string, string> locale in localizations.locales)
				{
					if (locale.Value == text)
					{
						_locale = locale.Key;
						break;
					}
				}
			}
			PlayerPrefs.SetString("selectedLanguage", _locale);
			_language = LocaleToLanguage(_locale);
			if (_language == "Arabic")
			{
				_language = EnglishLanguage;
			}
			if (localizations != null)
			{
				currentLocalization = localizations.localization[_language];
				if (localizations.fonts != null && localizations.fonts.Count == 0)
				{
					fonts.Add(localizations.fonts);
				}
				if (localizations.fontsCustomMaterials != null && localizations.fontsCustomMaterials.Count == 0)
				{
					fontMaterials.Add(localizations.fontsCustomMaterials);
				}
			}
			LocalizationTextMesh[] array2 = UnityEngine.Object.FindObjectsOfType<LocalizationTextMesh>();
			LocalizationLogo[] array3 = UnityEngine.Object.FindObjectsOfType<LocalizationLogo>();
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j].ChangeLanguageCommand();
			}
			for (int k = 0; k < array3.Length; k++)
			{
				array3[k].ChangeLanguageCommand();
			}
		}

		private string GetCurrentLocale()
		{
			string text = string.Empty;
			try
			{
				text = AppsMinistrySdkBinding.Instance.AppsMinistrySDKCurrentSystemLanguage();
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = EnglishLocale;
			}
			return text.ToLower();
		}

		private string LocaleToLanguage(string locale)
		{
			string result = EnglishLanguage;
			if (localizations.locales.ContainsKey(locale))
			{
				result = localizations.locales[locale];
			}
			else if (locale.Length > 2)
			{
				string key = locale.Substring(0, 2);
				if (localizations.locales.ContainsKey(key))
				{
					result = localizations.locales[key];
				}
				else
				{
					Debug.LogWarning("Language not Found " + locale);
				}
			}
			return result;
		}

		private void Load()
		{
			TextAsset textAsset = (TextAsset)Resources.Load("Localization/GameLocalize", typeof(TextAsset));
			if (textAsset != null)
			{
				localizations = JsonConvert.DeserializeObject<LocalizeLanguages>(textAsset.text);
			}
		}

		public bool ChangeFontByRule(string fontName, out string newFont)
		{
			if (currentLocalization != null && currentLocalization.rules != null)
			{
				newFont = string.Empty;
				Dictionary<string, string> dictionary = currentLocalization.rules.fonts;
				return dictionary != null && dictionary.TryGetValue(fontName, out newFont);
			}
			newFont = fontName;
			return false;
		}

		public bool ChangeAlignmentByRule(TextAlignment alignment, out TextAlignment newAlignment)
		{
			if (currentLocalization != null && currentLocalization.rules != null)
			{
				newAlignment = TextAlignment.Left;
				Dictionary<TextAlignment, TextAlignment> alignments = currentLocalization.rules.alignments;
				return alignments != null && alignments.TryGetValue(alignment, out newAlignment);
			}
			newAlignment = alignment;
			return false;
		}

		public string GetValueByKey(string key)
		{
			return GetValueByKey(key, string.Empty, false);
		}

		public string GetValueByKey(string key, string defaultValue)
		{
			return GetValueByKey(key, defaultValue, false);
		}

		public string GetValueByKey(string key, string defaultValue, bool ignoreArabicFormat)
		{
			if (currentLocalization != null && currentLocalization.localizeItems != null)
			{
				string value = string.Empty;
				if (currentLocalization.localizeItems.TryGetValue(key, out value))
				{
					if (ignoreArabicFormat)
					{
						return value;
					}
					return (!_arabic) ? value : ArabicFixer.Fix(value, false, false);
				}
			}
			return defaultValue;
		}

		public void LocalizeTextMesh(TextMesh textMesh, string key, string defaultValue = "")
		{
			if (textMesh != null)
			{
				textMesh.text = GetValueByKey(key, defaultValue);
				ConfigTextMesh(textMesh);
			}
		}

		public void ConfigTextMesh(TextMesh textMesh)
		{
			if (!(textMesh != null))
			{
				return;
			}
			string newFont = string.Empty;
			if (ChangeFontByRule(textMesh.font.name, out newFont) && !string.IsNullOrEmpty(newFont) && textMesh.font.name != newFont)
			{
				Font font = fonts[newFont];
				if (font != null)
				{
					Renderer component = textMesh.gameObject.GetComponent<Renderer>();
					Material material = textMesh.font.material;
					Material material2 = font.material;
					if (material.name != component.material.name)
					{
						Material material3 = fontMaterials[newFont];
						if (material3 != null)
						{
							material2 = material3;
						}
					}
					textMesh.font = font;
					component.material = material2;
				}
			}
			TextAlignment newAlignment = TextAlignment.Center;
			if (ChangeAlignmentByRule(textMesh.alignment, out newAlignment))
			{
				textMesh.alignment = newAlignment;
			}
		}

		public static string GetValue(string key, string defaultValue = "")
		{
			return MonoSingleton<Localization>.Instance.GetValueByKey(key, defaultValue);
		}
	}
}

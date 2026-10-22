using AppsMinistry.Core;
using Slugterra.Configuration;
using UnityEngine;

namespace Slugterra.UI.Shops
{
	public class SlugBarViewController : MonoBehaviour
	{
		[SerializeField]
		private SlugIconView _attackSlugIcon;

		[SerializeField]
		private SlugIconView _defenceSlugIcon;

		[SerializeField]
		private SlugIconView _specialSlugIcon;

		[SerializeField]
		private TextMesh _categoryName;

		[SerializeField]
		private tk2dBaseSprite _frameSprite;

		private void Awake()
		{
			MonoSingleton<Localization>.Instance.ConfigTextMesh(_categoryName);
		}

		public void Select(SlugGroup group)
		{
			_attackSlugIcon.Select(group == SlugGroup.Attack);
			_defenceSlugIcon.Select(group == SlugGroup.Control);
			_specialSlugIcon.Select(group == SlugGroup.Special);
			if (_frameSprite != null && _categoryName != null)
			{
				_frameSprite.gameObject.SetActive(true);
				_categoryName.gameObject.SetActive(true);
				_categoryName.text = GetCategoryName(group);
			}
		}

		public void Diselect()
		{
			if (_frameSprite != null && _categoryName != null)
			{
				_frameSprite.gameObject.SetActive(false);
				_categoryName.gameObject.SetActive(false);
			}
			_attackSlugIcon.Select(false);
			_defenceSlugIcon.Select(false);
			_specialSlugIcon.Select(false);
		}

		private string GetCategoryName(SlugGroup group)
		{
			switch (group)
			{
			case SlugGroup.Attack:
				return Localization.GetValue("slug_attack_section_title", string.Empty);
			case SlugGroup.Control:
				return Localization.GetValue("slug_control_section_title", string.Empty);
			case SlugGroup.Special:
				return Localization.GetValue("slug_special_section_title", string.Empty);
			default:
				return string.Empty;
			}
		}
	}
}

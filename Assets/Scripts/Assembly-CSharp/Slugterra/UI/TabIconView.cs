using AppsMinistry.UI;
using UnityEngine;

namespace Slugterra.UI
{
	public class TabIconView : UIView
	{
		[SerializeField]
		private tk2dUIToggleButton _toggleButton;

		[SerializeField]
		private tk2dSprite _icon;

		[SerializeField]
		private TextMesh _label;

		[SerializeField]
		private string _selectedBackgroundName = "btn_orange";

		[SerializeField]
		private string _selectedIconName;

		[SerializeField]
		private GameObject _selectedGlow;

		[SerializeField]
		private Color _selectedLabelColor = new Color32(byte.MaxValue, 179, 0, byte.MaxValue);

		private string _normalIconName;

		private Color _normalLabelColor;

		private void Awake()
		{
			if (_toggleButton != null)
			{
				_toggleButton.IsOn = false;
				return;
			}
			if (_icon != null)
			{
				_normalIconName = _icon.GetSpriteName();
			}
			if (_label != null)
			{
				_normalLabelColor = _label.color;
			}
			if (_selectedGlow != null)
			{
				_selectedGlow.SetActive(false);
			}
		}

		protected override void OnApplyModel(object model)
		{
			if (!(model is bool))
			{
				return;
			}
			if (_toggleButton != null)
			{
				_toggleButton.IsOn = (bool)model;
				return;
			}
			if ((bool)model)
			{
				if (_icon != null)
				{
					_icon.SetSprite(_selectedIconName);
				}
				if (_label != null)
				{
					_label.color = _selectedLabelColor;
				}
			}
			else
			{
				if (_icon != null)
				{
					_icon.SetSprite(_normalIconName);
				}
				if (_label != null)
				{
					_label.color = _normalLabelColor;
				}
			}
			if (_selectedGlow != null)
			{
				_selectedGlow.SetActive((bool)model);
			}
		}
	}
}
